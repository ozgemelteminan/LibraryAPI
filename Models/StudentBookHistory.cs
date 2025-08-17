namespace LibraryApi.Models
{
    public class StudentBookHistory
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public Student? Student { get; set; }
        public Book? Book { get; set; }
    }
}
