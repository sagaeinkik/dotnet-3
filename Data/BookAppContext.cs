using BookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Data;

public class BookAppContext : DbContext
{
    //Konstruktor
    public BookAppContext(DbContextOptions<BookAppContext> options)
        : base(options)
    {
    }

    //Skapa tabellerna
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
}