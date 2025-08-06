using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = null!;

        [Required(ErrorMessage = "LibraryId is required")]
        public int LibraryId { get; set; }

        public Library? Library { get; set; }

        public ICollection<StudentBook>? StudentBooks { get; set; }
    }
}
