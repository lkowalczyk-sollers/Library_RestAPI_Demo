using Library_demo.Data;
using Library_demo.Interfaces;
using Library_demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_demo.Repositories
{
    public class BookRepository(DataContext context) : IBookRepository
    {
        public bool BookExists(int id)
        {
            return context.Books.Any(b => b.Id == id);
        }

        public bool CreateBook(Book book, int lenderId, int authorId)
        {
            var lenders = context.Users.Where(u => u.Id == lenderId);
            if(lenders.Any())
            {
                book.Lender = lenders.FirstOrDefault();
            }

            var authors = context.Authors.Where(a => a.Id == authorId);

            if(authors.Any())
            {
                if (!context.BookAuthorLinks.Where(b => b.AuthorId == authorId && b.BookId == book.Id).Any())
                {
                    var link = new BookAuthorLink()
                    {
                        Author = authors.FirstOrDefault(),
                        Book = book
                    };
                    context.Add(link);
                }
            }

            context.Add(book);
            return Save();
        }

        public bool DeleteBook(Book book)
        {
            var links = context.BookAuthorLinks.Where(ba => ba.BookId == book.Id);
            if(links.Any())
            {
                context.RemoveRange(links);
            }
            context.Remove(book);
            return Save();
        }

        public Book GetBook(int id)
        {
            return context.Books.Where(b => b.Id == id).FirstOrDefault();
        }

        public ICollection<Book> GetBooks()
        {
            return context.Books.ToList();
        }

        public bool UpdateBook(Book book)
        {
            context.Update(book);
            return Save();
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }

        public ICollection<Book> GetBooksByAuthorName(string authorName)
        {
            var generalizedName = authorName.Replace(" ", "").ToLower();
            return context.BookAuthorLinks.Where(ba => ba.Author.Name.Replace(" ", "").ToLower() == generalizedName).Select(ba => ba.Book).ToList();
        }

        public User GetLender(int bookId)
        {
            return context.Users.Where(u => u.Id == context.Books.Where(b => b.Id == bookId).FirstOrDefault().Lender.Id).FirstOrDefault();
        }

        public bool ReturnBook(int bookId)
        {
            var book = context.Books.Include(b => b.Lender).FirstOrDefault(b => b.Id == bookId);
            book.Lender = null;
            context.Update(book);
            return Save();
        }
    }
}
