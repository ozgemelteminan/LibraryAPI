using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Book
    {
        // Primary Key - Unique identifier for the book
        public int Id { get; set; }


        // Title of the book (Required)
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;


        // Author of the book (Required)
        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = null!;


        // ISBN of the book. 
        public string ISBN { get; set; } = null!;


        // Foreign Key - References the Library this book belongs to
        [Required(ErrorMessage = "LibraryId is required")]
        public int LibraryId { get; set; }


        // Navigation Property - The Library that owns this book
        public Library? Library { get; set; }


        // Navigation Property - Borrow history (students who borrowed this book)
        public ICollection<StudentBook>? StudentBooks { get; set; }


        // Year the book was published (Required)
        [Required(ErrorMessage = "Year is required")]
        public int Year { get; set; }

    }
}
