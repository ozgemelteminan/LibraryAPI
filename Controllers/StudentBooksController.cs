using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    // Controller responsible for student-book operations:
    // - See who borrowed which book
    // - Borrow a new book
    // - Return a borrowed book
    // - See borrowing history of a student
    
    [ApiController]
    [Route("api/[controller]")]
    public class StudentBooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        // Constructor: injects database context
        public StudentBooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/studentbooks
        // Returns a list of all currently borrowed books
        // Includes student info and book info
        [HttpGet]
        public IActionResult GetStudentBooks()
        {
            var studentBooks = _context.StudentBooks
                .Include(sb => sb.Student) // also load student details
                .Include(sb => sb.Book)    // also load book details
                .Select(sb => new
                {
                    sb.Id,
                    StudentId = sb.Student!.Id,
                    StudentName = sb.Student!.FullName,
                    BookId = sb.Book!.Id,
                    BookTitle = sb.Book!.Title,
                    sb.BorrowDate
                })
                .ToList();

            return Ok(studentBooks);
        }


        // POST: api/studentbooks
        // Allows a student to borrow a new book
        // Validates:
        // - student exists
        // - book exists
        // - book is not already borrowed
        // - same student is not borrowing same book again        
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

            // Check if book is already borrowed
            var isBookAlreadyBorrowed = _context.StudentBooks
                .Any(sb => sb.BookId == dto.BookId);

            if (isBookAlreadyBorrowed)
                return Conflict("This book is already borrowed by another student.");

            // Check if student tries to borrow same book again without returning
            var isStudentAlreadyBorrowingSameBook = _context.StudentBooks
                .Any(sb => sb.StudentId == dto.StudentId && sb.BookId == dto.BookId);

            if (isStudentAlreadyBorrowingSameBook)
                return Conflict("The student is already borrowing this book and has not returned it yet.");

            var studentBook = new StudentBook
            {
                StudentId = dto.StudentId,
                BookId = dto.BookId,
                BorrowDate = DateTime.UtcNow
            };

            _context.StudentBooks.Add(studentBook);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudentBooks), new { id = studentBook.Id }, studentBook);
        }

        // DELETE: api/studentbooks/{studentId}/{bookId}
        // Allows a student to return a book
        // Moves record from "active borrowed books" to "history"
        [HttpDelete("{studentId}/{bookId}")]
        public IActionResult ReturnBookByStudentAndBook(int studentId, int bookId)
        {
            var studentBook = _context.StudentBooks
                .Include(sb => sb.Book)
                .Include(sb => sb.Student)
                .FirstOrDefault(sb => sb.StudentId == studentId && sb.BookId == bookId);

            if (studentBook == null)
                return NotFound($"No record found for Student {studentId} and Book {bookId}");

            // Save this transaction to history table
            var history = new StudentBookHistory
            {
                StudentId = studentBook.StudentId,
                BookId = studentBook.BookId,
                BorrowDate = studentBook.BorrowDate,
                ReturnDate = DateTime.UtcNow
            };

            // Remove from current borrowed books
            _context.StudentBookHistories.Add(history);
            _context.StudentBooks.Remove(studentBook);
            _context.SaveChanges();

            return NoContent();
        }

        // GET: api/studentbooks/history/{studentId}
        // Returns borrowing history for a given student
        // Shows all books borrowed and return dates
        [HttpGet("history/{studentId}")]
        public IActionResult GetStudentBookHistory(int studentId)
        {
            var history = _context.StudentBookHistories
                .Include(sb => sb.Book)
                .Where(sb => sb.StudentId == studentId)
                .Select(sb => new
                {
                    sb.Id,
                    BookId = sb.Book!.Id,
                    BookTitle = sb.Book!.Title,
                    sb.BorrowDate,
                    sb.ReturnDate
                })
                .OrderByDescending(sb => sb.BorrowDate)
                .ToList();

            return Ok(history);
        }
    }
}
