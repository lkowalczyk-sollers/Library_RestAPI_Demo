using AutoMapper;
using Library_demo.Data;
using Library_demo.Dtos;
using Library_demo.Interfaces;
using Library_demo.Models;

namespace Library_demo.Repositories
{
    public class AuthorRepository(DataContext context) : IAuthorRepository
    {
        public bool AuthorExists(int id)
        {
            return context.Authors.Any(a => a.Id == id);
        }

        public bool CreateAuthor(Author author, int bookId)
        {
            var books = context.Books.Where(a => a.Id == bookId);

            if (books.Any())
            {
                if (!context.BookAuthorLinks.Where(b => b.AuthorId == author.Id && b.BookId == bookId).Any())
                {
                    var link = new BookAuthorLink()
                    {
                        Author = author,
                        Book = books.FirstOrDefault(),
                    };
                    context.Add(link);
                }
            }

            context.Add(author);

            return Save();
        }

        public bool DeleteAuthor(Author author)
        {
            var links = context.BookAuthorLinks.Where(ba => ba.AuthorId == author.Id);
            if (links.Any())
            {
                context.RemoveRange(links);
            }
            context.Remove(author);
            return Save();
        }

        public Author GetAuthor(int id)
        {
            return context.Authors.Where(a => a.Id == id).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return context.Authors.ToList();
        }

        public bool UpdateAuthor(Author author)
        {
            context.Update(author);
            return Save();
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }

        public ICollection<Author> GetAuthorsByBookTitle(string title)
        {
            var generalizedName = title.Replace(" ", "").ToLower();
            return context.BookAuthorLinks.Where(ba => ba.Book.Title.Replace(" ", "").ToLower() == generalizedName).Select(ba => ba.Author).ToList();
        }
    }
}
