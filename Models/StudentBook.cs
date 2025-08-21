namespace LibraryApi.Models
{
    public class StudentBook
    {
        // Primary key - unique identifier for each record
        public int Id { get; set; }


        // Foreign key - references the Student entity
        public int StudentId { get; set; }


        // Foreign key - references the Book entity
        public int BookId { get; set; }


        // Navigation property - related Student entity
        public Student? Student { get; set; }


        // Navigation property - related Book entity
        public Book? Book { get; set; }


        // The date when the book was borrowed (default = current UTC time)
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
    }
}
