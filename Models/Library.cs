namespace LibraryApi.Models
{
    public class Library
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public ICollection<Book>? Books { get; set; }
    }
}
