using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public StudentsController(LibraryContext context)
        {
            _context = context;
        }

        // Tüm öğrencileri getir
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        // Tek öğrenci getir (GET /api/students/{id})
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // Yeni öğrenci kaydı (register)
        [HttpPost]
        public IActionResult Register([FromBody] Student student)
        {
            var existing = _context.Students.FirstOrDefault(s => s.Email == student.Email);
            if (existing != null)
            {
                return BadRequest("Bu e-posta zaten kayıtlı.");
            }

            _context.Students.Add(student);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // Giriş (login)
        [HttpPost("login")]
        public IActionResult Login([FromBody] StudentLoginDto loginDto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var student = _context.Students
        .FirstOrDefault(s => s.Email == loginDto.Email && s.Password == loginDto.Password);

    if (student == null)
        return Unauthorized("Invalid email or password.");

    return Ok(student);
        }
    }
}
