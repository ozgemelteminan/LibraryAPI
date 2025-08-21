namespace LibraryApi.Models
{
    public class StudentBookHistory
    {
       // Primary key - unique identifier for each recor
        public int Id { get; set; }


        // Foreign key - references the Student entity
        public int StudentId { get; set; }


        // Foreign key - references the Book entity
        public int BookId { get; set; }


        // The date when the book was borrowed
        public DateTime BorrowDate { get; set; }


        // The date when the book was returned
        public DateTime ReturnDate { get; set; }


        // Navigation property - related Student entity
        public Student? Student { get; set; }


        // Navigation property - related Book entity
        public Book? Book { get; set; }
    }
}
