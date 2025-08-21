using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Models.DTOs;  


namespace LibraryApi.Controllers
{
    // Controller responsible for library management:
    // - Get all libraries
    // - Get a single library
    // - Create a new library
    // - Update an existing library

    [ApiController]
    [Route("api/[controller]")]
    public class LibrariesController : ControllerBase
    {
        private readonly LibraryContext _context;


        // Constructor: injects database context
        public LibrariesController(LibraryContext context)
        {
            _context = context;
        }


        // GET: api/libraries
        // Returns a list of all libraries
        [HttpGet]
        public IActionResult GetLibraries()
        {
            var libraries = _context.Libraries.ToList();
            return Ok(libraries);  // return libraries as JSON
        }


        // GET: api/libraries/{id}
        // Returns a single library by ID
        // If not found -> 404 NotFound
        [HttpGet("{id}")]
        public IActionResult GetLibrary(int id)
        {
            var library = _context.Libraries.Find(id);
            if (library == null)
                return NotFound();

            return Ok(library);
        }


        // POST: api/libraries
        // Creates a new library
        // Returns 201 Created with location header
        [HttpPost]
        public IActionResult CreateLibrary(Library library)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Libraries.Add(library);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, library);
        }


        // PUT: api/libraries/{id}
        // Updates an existing library
        // - Validates ID match
        // - Returns 404 if library not found
        // - Returns 204 NoContent if update successful
        [HttpPut("{id}")]
        public IActionResult UpdateLibrary(int id, Library updatedLibrary)
        {
            if (id != updatedLibrary.Id)
                return BadRequest("ID mismatch.");

            var library = _context.Libraries.Find(id);
            if (library == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // update properties
            library.Name = updatedLibrary.Name;
            library.Location = updatedLibrary.Location;

            _context.SaveChanges();

            return NoContent();  // success, no content returned
        }
    }
}
