using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewBookish.Models;
using NewBookish.Data;
using NewBookish.Helpers;
using NewBookish.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NewBookish.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookishContext _dbContext;

        public HomeController(ILogger<HomeController> logger, BookishContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LibraryManagement()
        {
            return View();
        }

        public async Task<IActionResult> Index(
            string currentTitle,
            string currentAuthor,
            string sortOrder,
            string searchTitle,
            string searchAuthor,
            int? page)
        {

            if (_dbContext.Books == null)
            {
                TempData["Error"] = "true";
                TempData["Message"] = "Entity set 'BookishContext.Books' is null.";
                return View();
            }

            int pageSize = 10;

            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["AuthorSortParm"] = sortOrder == "Author" ? "author_desc" : "Author";

            if (searchTitle != null || searchAuthor != null)
            {
                page = 1;
            }
            else
            {
                searchTitle = currentTitle;
                searchAuthor = currentAuthor;
            }

            ViewData["TitleFilter"] = searchTitle;
            ViewData["AuthorFilter"] = searchAuthor;

            var books = from b in _dbContext.Books
                        select b;


            if (!string.IsNullOrEmpty(searchTitle))
            {
                books = books.Where(s => s.Title!.ToLower().Contains(searchTitle.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchAuthor))
            {
                books = books.Where(s => s.Author!.ToLower().Contains(searchAuthor.ToLower()));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(s => s.Title.ToLower());
                    break;
                case "Author":
                    books = books.OrderBy(s => s.Author.ToLower());
                    break;
                case "author_desc":
                    books = books.OrderByDescending(s => s.Author.ToLower());
                    break;
                default:
                    books = books.OrderBy(s => s.Title.ToLower());
                    break;
            }

            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), page ?? 1, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string BookTitle, string BookAuthor)
        {
            if (string.IsNullOrEmpty(BookTitle) || string.IsNullOrEmpty(BookAuthor))
            {
                TempData["Error"] = "true";
                TempData["Message"] = "Book information is not provided. Please try again.";
                return View();
            }
            var book = _dbContext.Books.FirstOrDefault(b => b.Title.ToLower() == BookTitle.ToLower() && b.Author.ToLower() == BookAuthor.ToLower());
            if (book != null)
            {
                TempData["Error"] = "true";
                TempData["Message"] = "Book already exists. Please try again.";
                return View();
            }
            _dbContext.Add(new Book { Title = BookTitle, Author = BookAuthor, NoOfCopies = 1, AvailableCopies = 1 });
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Book is added successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            var book = _dbContext.Books.Find(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult EditPartial(int id)
        {
            var book = _dbContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return PartialView("EditBookPartial", book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Book model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Author))
            {
                return Json(new { success = false, message = "Title and Author are required." });
            }

            var book = _dbContext.Books.Find(model.Id);
            if (book == null)
            {
                return Json(new { success = false, message = "Book not found." });
            }

            var duplicateBook = _dbContext.Books.FirstOrDefault
            (b => b.Title.ToLower() == book.Title.ToLower() &&
            b.Author.ToLower() == book.Author.ToLower() &&
            b.Id != book.Id);
            if (duplicateBook != null)
            {
                return Json(new { success = false, message = "A book with the same title and author already exists." });
            }

            // Update the book details
            book.Title = model.Title;
            book.Author = model.Author;
            book.NoOfCopies = model.NoOfCopies;
            book.AvailableCopies = model.AvailableCopies;

            _dbContext.SaveChanges();

            // Return the updated book data
            return Json(new
            {
                success = true,
                data = new
                {
                    title = book.Title,
                    author = book.Author,
                    noOfCopies = book.NoOfCopies,
                    availableCopies = book.AvailableCopies
                }
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
