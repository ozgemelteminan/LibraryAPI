using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrariesController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LibrariesController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/libraries
        [HttpGet]
        public IActionResult GetLibraries()
        {
            var libraries = _context.Libraries.ToList();
            return Ok(libraries);
        }

        // GET: api/libraries/5
        [HttpGet("{id}")]
        public IActionResult GetLibrary(int id)
        {
            var library = _context.Libraries.Find(id);
            if (library == null)
                return NotFound();

            return Ok(library);
        }

        // POST: api/libraries
        [HttpPost]
        public IActionResult CreateLibrary(Library library)
        {
            _context.Libraries.Add(library);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, library);
        }
    }
}
