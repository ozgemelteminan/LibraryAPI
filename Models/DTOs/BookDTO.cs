namespace LibraryApi.Models.DTOs
{
    // Data Transfer Object (DTO) for transferring book data
    public class BookDto
    {
        // Unique identifier of the book
        public int Id { get; set; }

        // Title of the book
        public string Title { get; set; } = null!;

        // Author of the book
        public string Author { get; set; } = null!;

        // Foreign key: Id of the library where the book belongs
        public int LibraryId { get; set; }

        // Publication year of the book
        public int Year { get; set; }

        // ISBN unique code for books
        public string Isbn { get; set; } = null!;
    }
}
