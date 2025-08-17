using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IConfiguration _configuration;

        public StudentsController(LibraryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Tüm öğrencileri getir
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        // Tek öğrenci getir
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
        [HttpPost("register")]
        public IActionResult Register([FromBody] Student student)
        {
            if (string.IsNullOrEmpty(student.Password))
                return BadRequest("Password cannot be null or empty.");

            var existing = _context.Students.FirstOrDefault(s => s.Email == student.Email);
            if (existing != null)
            {
                return BadRequest("Bu e-posta zaten kayıtlı.");
            }

            _context.Students.Add(student);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        // Giriş (login) + JWT token üret
        [HttpPost("login")]
        public IActionResult Login([FromBody] StudentLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(loginDto.Password))
                return BadRequest("Password cannot be null or empty.");

            var student = _context.Students
                .FirstOrDefault(s => s.Email == loginDto.Email && s.Password == loginDto.Password);

            if (student == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // ✅ JWT oluştur
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, student.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, student.Email),
                new Claim("fullName", student.FullName)
            };

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is missing in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ✅ Hem kullanıcı bilgisi hem token dön
            return Ok(new
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                Token = tokenString
            });
        }
    }
}
