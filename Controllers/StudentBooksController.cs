using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Models.DTOs; // Eğer DTO kullandıysan
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentBooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public StudentBooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET /api/studentbooks
        [HttpGet]
        public IActionResult GetStudentBooks()
        {
            var studentBooks = _context.StudentBooks
                .Include(sb => sb.Student)
                .Include(sb => sb.Book)
                .Select(sb => new 
                {
                    sb.Id,
                    StudentId = sb.Student!.Id,
                    StudentName = sb.Student!.FullName,
                    BookId = sb.Book!.Id,
                    BookTitle = sb.Book!.Title
                })
                .ToList();

            return Ok(studentBooks);
        }

        // POST /api/studentbooks
        [HttpPost]
        public IActionResult CreateStudentBook(StudentBookDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentExists = _context.Students.Any(s => s.Id == dto.StudentId);
            var bookExists = _context.Books.Any(b => b.Id == dto.BookId);

            if (!studentExists)
                return NotFound($"Student with ID {dto.StudentId} not found.");

            if (!bookExists)
                return NotFound($"Book with ID {dto.BookId} not found.");

            var studentBook = new StudentBook
            {
                StudentId = dto.StudentId,
                BookId = dto.BookId
            };

            _context.StudentBooks.Add(studentBook);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudentBooks), new { id = studentBook.Id }, studentBook);
        }

        // DELETE /api/studentbooks/{id}
        [HttpDelete("{id}")]
        public IActionResult ReturnBook(int id)
        {
            var studentBook = _context.StudentBooks.Find(id);

            if (studentBook == null)
                return NotFound($"No record found with ID {id}");

            _context.StudentBooks.Remove(studentBook);
            _context.SaveChanges();

            return NoContent(); // 204
        }
    }
}
