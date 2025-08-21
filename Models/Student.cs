using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Student
    {
        // Primary key - unique identifier for each student
        public int Id { get; set; }


        // Full name of the student (required)
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = null!;


        // Email address of the student (required, should be unique in real use cases)
        [Required(ErrorMessage = "Email is required")]
         public string Email { get; set; } = null!;


        // Password for authentication (required, should be stored securely in practice)
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;


        // Navigation property - collection of books borrowed by this student
        public ICollection<StudentBook>? StudentBooks { get; set; }
    }
}
