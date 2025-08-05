namespace LibraryApi.Models
{
    public class StudentBook
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
