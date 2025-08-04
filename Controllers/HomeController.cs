using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewBookish.Models;
using NewBookish.Data;

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LibraryManagement()
        {
            return View();
        }

        public IActionResult Catalogue(string searchTitle, string searchAuthor)
        {
            if (_dbContext.Books == null)
            {
                return View("Entity set 'BookishContext.Books'  is null.");
            }

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
            return View(books.ToList());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
