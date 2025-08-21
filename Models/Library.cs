using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Library
    {
        // Primary key - unique identifier for each library
        public int Id { get; set; }


        // Name of the library (required)
        [Required]
        public string Name { get; set; } = null!;


        // Physical location/address of the library (required)
        [Required]
        public string Location { get; set; } = null!;


        // Navigation property - collection of books available in this library
        public ICollection<Book>? Books { get; set; }
    }
}
