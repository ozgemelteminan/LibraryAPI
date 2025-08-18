namespace LibraryApi.Models.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int LibraryId { get; set; }

        public int Year { get; set; }     // ✅ eklendi
        public string Isbn { get; set; } = null!; // ✅ eklendi
    }
}
