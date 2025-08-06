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

/*         public IActionResult Index()
        {
            return View();
        } */

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
                    books = books.OrderByDescending(s => s.Title);
                    break;
                case "Author":
                    books = books.OrderBy(s => s.Author);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(s => s.Author);
                    break;
                default:
                    books = books.OrderBy(s => s.Title);
                    break;
            }
            
            return View(await PaginatedList<Book>.CreateAsync(books.AsNoTracking(), page ?? 1, pageSize));
        }

        [HttpPost] 
        public IActionResult Index(string BookTitle, string BookAuthor)
        {
            if (string.IsNullOrEmpty(BookTitle) || string.IsNullOrEmpty(BookAuthor))
            {
                TempData["Error"] = "true";
                TempData["Message"] = "Book information is not provided. Please try again.";
                return View();
            }
            var book = _dbContext.Books.FirstOrDefault(b => b.Title == BookTitle && b.Author == BookAuthor);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
