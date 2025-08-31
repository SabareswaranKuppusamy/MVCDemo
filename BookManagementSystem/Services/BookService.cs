using BookManagementSystem.Models;
using BookManagementSystem.Services;

namespace BookManagementSystem.Data
{
    public class BookService : IBookService
    {
        private static readonly List<Book> _books = new List<Book> {};
        private static int _nextId = 1;

        public BookService()
        {             
        }

        public Book? GetById(int id)
        {
            var book = _books.FirstOrDefault(x => x.Id == id);

            return book;
        }

        public List<Book>? GetAll()
        {
            var bookList = _books.ToList();

            return bookList;
        }

        public void Add(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
             
            book.Id = _nextId++;
            _books.Add(book);
        }

        public void Update(Book book)
        {
            var existingBook = GetById(book.Id);

            if(existingBook == null)
            {
                return;
            }

            existingBook.Title = book.Title;  
            existingBook.Author = book.Author;  
            existingBook.Price = book.Price;  
            existingBook.Description = book.Description;  
        }

        public void Delete(int id)
        {
            var existingBook = GetById(id);

            if (existingBook != null )
            {
                _books.Remove(existingBook);
            }
        }
    }
}