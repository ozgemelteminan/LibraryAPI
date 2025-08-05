namespace LibraryApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int LibraryId { get; set; }
        public Library? Library { get; set; }
        public ICollection<StudentBook>? StudentBooks { get; set; }
    }
}
