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

        public IActionResult Index()
        {

            var books = _bookService.GetAll() ?? new List<Book>();

            if(books is null)
            {
                return View(new List<Book>());
            } 

            return View(books);
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
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Failed to create book. Please check the form.";
                    return View(book);
                }

                _bookService.Add(book);

                TempData["SuccessMessage"] = "Book created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error create book details");
                TempData["ErrorMessage"] = "An error occurred while creating the book.";
                return RedirectToAction(nameof(Index));
            }
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error edit book details");
                TempData["ErrorMessage"] = "An error occurred while editing the book.";
                return RedirectToAction(nameof(Index));
            } 
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting the book"); 
                TempData["ErrorMessage"] = "An error occurred while deleting the book.";
                return RedirectToAction(nameof(Index));
            } 
        }


        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["ErrorMessage"] = "Book ID was not provided.";
                return View("NotFound");
            }
            try
            {
                var book = _bookService.GetById(id.Value);

                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {id}.";
                    return View("NotFound");
                }

                return View(book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading the book details");
                TempData["ErrorMessage"] = "An error occurred while loading the book details.";
                return View("Error");
            }
        }
    }
}
