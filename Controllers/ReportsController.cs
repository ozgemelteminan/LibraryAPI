using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers

    // Controller responsible for reporting:
    // - Get all books borrowed by a specific student
    // - Get all books available in a specific library
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryContext _context;

        // Constructor: injects database context
        public ReportsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: /api/reports/student/{studentId}
        // Returns all books currently borrowed by a given student
        // Includes book info: Id, Title, Author, LibraryId
        [HttpGet("student/{studentId}")]
        public IActionResult GetBooksByStudent(int studentId)
        {
            var books = _context.StudentBooks
                .Include(sb => sb.Book)                 // also load book details
                .Where(sb => sb.StudentId == studentId) // also load book details
                .Select(sb => new
                {
                    sb.Book!.Id,
                    sb.Book.Title,
                    sb.Book.Author,
                    sb.Book.LibraryId
                })
                .ToList();

            return Ok(books);    // return as JSON
        }

        // GET: /api/reports/library/{libraryId}
        // Returns all books available in a given library
        // Includes book info: Id, Title, Author, LibraryId
        [HttpGet("library/{libraryId}")]
        public IActionResult GetBooksByLibrary(int libraryId)
        {
            var books = _context.Books
                .Where(b => b.LibraryId == libraryId)  // filter by library
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.LibraryId
                })
                .ToList();

            return Ok(books);   // return as JSON
        }
    }
}
