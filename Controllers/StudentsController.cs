using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryApi.Controllers
{
    // Controller responsible for student operations:
    // - Retrieve all students
    // - Retrieve a single student
    // - Register new students
    // - Login and generate JWT tokens

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IConfiguration _configuration;

        // Constructor: injects database context and configuration (used for JWT settings)
        public StudentsController(LibraryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/students
        // Returns a list of all students in the system
        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        // GET: api/students/{id}
        // Returns a single student by ID
        // If student not found -> 404 NotFound
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

       // POST: api/students/register
        // Registers a new student
        // Validates email uniqueness and non-empty password
        // Returns 201 Created on success
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

        // POST: api/students/login
        // Authenticates a student by email and password
        // If valid, generates a JWT token with student info
        // If invalid, returns 401 Unauthorized
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


            // Create JWT claims with student info
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, student.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, student.Email),
                new Claim("fullName", student.FullName)
            };

            // Get JWT key from configuration
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is missing in configuration");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create JWT token with claims and expiration
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return student info + token
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
