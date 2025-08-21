using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    // Controller responsible for book management:
    // - Get all books
    // - Create a new book
    // - Update an existing book

    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;


        // Constructor: injects database context
        public BooksController(LibraryContext context)
        {
            _context = context;
        }


        // GET: /api/books
        // Returns a list of all books (mapped to BookDto)
        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _context.Books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    LibraryId = b.LibraryId,
                    Year = b.Year,
                    Isbn = b.ISBN
                })
                .ToList();

            return Ok(books);
        }


        // POST: /api/books
        // Creates a new book
        // - Validates model state
        // - Maps from BookDto -> Book entity
        // - Returns 201 Created with new book data
        [HttpPost]
        public IActionResult CreateBook(BookDto newBook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var book = new Book
            {
                Title = newBook.Title,
                Author = newBook.Author,
                LibraryId = newBook.LibraryId,
                Year = newBook.Year,
                ISBN = newBook.Isbn
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                LibraryId = book.LibraryId,
                Year = book.Year,
                Isbn = book.ISBN
            };

            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, bookDto);
        }


        // PUT: /api/books/{id}
        // Updates an existing book
        // - Validates ID match
        // - Returns 404 if book not found
        // - Updates book properties and saves changes
        // - Returns 204 NoContent if successful
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, BookDto updatedBookDto)
        {
            if (id != updatedBookDto.Id)
                return BadRequest("ID mismatch.");

            var book = _context.Books.Find(id);
            if (book == null)
                return NotFound();

            // update properties
            book.Title = updatedBookDto.Title;
            book.Author = updatedBookDto.Author;
            book.LibraryId = updatedBookDto.LibraryId;
            book.Year = updatedBookDto.Year;
            book.ISBN = updatedBookDto.Isbn;

            _context.SaveChanges();

            return NoContent();
        }
    }
}
