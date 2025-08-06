using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ReportsController(LibraryContext context)
        {
            _context = context;
        }

        // GET /api/reports/student/{studentId}
        [HttpGet("student/{studentId}")]
        public IActionResult GetBooksByStudent(int studentId)
        {
            var books = _context.StudentBooks
                .Include(sb => sb.Book)
                .Where(sb => sb.StudentId == studentId)
                .Select(sb => new
                {
                    sb.Book!.Id,
                    sb.Book.Title,
                    sb.Book.Author,
                    sb.Book.LibraryId
                })
                .ToList();

            return Ok(books);
        }

        // GET /api/reports/library/{libraryId}
        [HttpGet("library/{libraryId}")]
        public IActionResult GetBooksByLibrary(int libraryId)
        {
            var books = _context.Books
                .Where(b => b.LibraryId == libraryId)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.LibraryId
                })
                .ToList();

            return Ok(books);
        }
    }
}
