using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;

namespace LibraryApi.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Library> Libraries => Set<Library>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<StudentBook> StudentBooks => Set<StudentBook>();
    }
}
