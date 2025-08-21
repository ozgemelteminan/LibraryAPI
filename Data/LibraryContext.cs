using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;

namespace LibraryApi.Data
{
    // DbContext is the main class that manages the database connection
    // and maps classes (entities) to database tables.
    public class LibraryContext : DbContext
    {
        // Constructor: takes DbContextOptions and passes them to the base DbContext
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }


        // Each DbSet<T> represents a table in the database
        // Students table
        public DbSet<Student> Students => Set<Student>();

         // Libraries table
        public DbSet<Library> Libraries => Set<Library>();

        // Books table
        public DbSet<Book> Books => Set<Book>();

        // StudentBooks table (relationship between Students and Books)
        public DbSet<StudentBook> StudentBooks => Set<StudentBook>();

        // StudentBookHistories table (history of borrowed/returned books)
        public DbSet<StudentBookHistory> StudentBookHistories => Set<StudentBookHistory>();
    }
}
