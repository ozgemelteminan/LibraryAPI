using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.DTOs
{   
     // Data Transfer Object (DTO) for transferring library data
     public class LibraryDto
    {
        // Unique identifier of the library (Required)
        [Required]
        public int Id { get; set; }

        // Name of the library (Required)
        [Required]
        public string Name { get; set; } = null!;

        // Physical location/address of the library (Required)
        [Required]
        public string Location { get; set; } = null!;
    }
}
