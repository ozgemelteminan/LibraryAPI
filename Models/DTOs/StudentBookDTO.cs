using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.DTOs
{
 // Data Transfer Object (DTO) for borrowing a book by a student
public class StudentBookDto
    {
        // The ID of the student who borrows the book
        [Required]
        public int StudentId { get; set; }


        // The ID of the borrowed book
        [Required]
        public int BookId { get; set; }
    }
}
