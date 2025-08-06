using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET /api/books
        [HttpGet]
        public IActionResult GetBooks()
{
    var books = _context.Books
        .Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            LibraryId = b.LibraryId
        })
        .ToList();

    return Ok(books);
        }

        // POST /api/books
        [HttpPost]
        public IActionResult CreateBook(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Books.Add(book);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }

        // PUT /api/books/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest("ID mismatch.");

            if (!_context.Books.Any(b => b.Id == id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(updatedBook).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }
    }
}
