using Library_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_demo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthorLink> BookAuthorLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthorLink>().HasKey(ba => new { ba.AuthorId, ba.BookId });

            modelBuilder.Entity<BookAuthorLink>().HasOne(a => a.Author)
                .WithMany(ba => ba.AuthorBooks)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<BookAuthorLink>().HasOne(b => b.Book)
                .WithMany(ba => ba.BookAuthors)
                .HasForeignKey(b => b.BookId);
        }

    }
}
