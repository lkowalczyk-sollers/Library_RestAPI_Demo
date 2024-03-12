using Library_demo.Models;

namespace Library_demo.Interfaces
{
    public interface IAuthorRepository
    {
        public ICollection<Author> GetAuthors();
        public Author GetAuthor(int id);
        public ICollection<Author> GetAuthorsByBookTitle(string title);
        public bool AuthorExists(int id);
        public bool CreateAuthor(Author author, int bookId);
        public bool UpdateAuthor(Author author);
        public bool DeleteAuthor(Author author);
        public bool Save();
    }
}
