namespace Library_demo.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecurityCode {  get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
