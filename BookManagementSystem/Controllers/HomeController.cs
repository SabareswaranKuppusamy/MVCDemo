using BookManagementSystem.Models;
using BookManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookService _bookService; 

        public HomeController(ILogger<HomeController> logger, IBookService bookstore)
        {
            _logger = logger;
            _bookService = bookstore;
        }

        public IActionResult Index(int page = 1, string searchTerm = "")
        {
            const int pageSize = 5;

            var books = _bookService.GetAll() ?? new List<Book>();

            if(books is null)
            {
                return View(new List<Book>());
            }

            if(!string.IsNullOrWhiteSpace(searchTerm))
            {
                books = books.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                         b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }


            var pagedBooks = books.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(books.Count / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;

            return View(pagedBooks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Failed to create book. Please check the form.";
                return View(book);
            }

            _bookService.Add(book);

            TempData["SuccessMessage"] = "Book created successfully!";
            return Redirect(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _bookService.GetById(id);

            if (book is null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        { 
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Failed to Update book. Please check the form.";
                return View(book);
            }

            _bookService.Update(book);
            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _bookService.GetById(id);

            if (book is null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _bookService.GetById(id);

            if (book != null)
            {
                _bookService.Delete(id);
                TempData["SuccessMessage"] = "Book deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Book not found.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
