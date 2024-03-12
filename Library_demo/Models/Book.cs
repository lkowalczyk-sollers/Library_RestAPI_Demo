namespace Library_demo.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<BookAuthorLink> BookAuthors { get; set; }

        public User Lender { get; set; }
    }
}
