namespace Library_demo.Models
{
    public class BookAuthorLink
    {
        public int BookId { get; set; }

        public int AuthorId { get; set; }

        public Book Book { get; set; }

        public Author Author { get; set; }
    }
}
