namespace Library_demo.Models
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<BookAuthorLink> AuthorBooks { get; set; }
    }
}
