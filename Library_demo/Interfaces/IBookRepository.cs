using Library_demo.Models;

namespace Library_demo.Interfaces
{
    public interface IBookRepository
    {
        public ICollection<Book> GetBooks();
        public Book GetBook(int id);

        public User GetLender(int bookId);

        public bool ReturnBook(int bookId);

        public ICollection<Book> GetBooksByAuthorName(string authorName);
        public bool BookExists(int id);
        public bool CreateBook(Book book, int lenderId, int authorId);
        public bool UpdateBook(Book book);
        public bool DeleteBook(Book book);
        public bool Save();
    }
}
