// Models/DTOs/BookDto.cs
namespace LibraryApi.Models.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int LibraryId { get; set; }
    }
}
