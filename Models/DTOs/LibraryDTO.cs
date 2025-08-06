// Models/DTOs/LibraryDto.cs
namespace LibraryApi.Models.DTOs
{
    public class LibraryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}
