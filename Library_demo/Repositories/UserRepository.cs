using Library_demo.Data;
using Library_demo.Interfaces;
using Library_demo.Models;

namespace Library_demo.Repositories
{
    public class UserRepository(DataContext context) : IUserRepository
    {
        public bool CreateUser(User user)
        {
            context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            var lentBooks = context.Books.Where(b => b.Lender.Id == user.Id);
            if(lentBooks.Any()) 
            {
                foreach (var book in lentBooks)
                {
                    book.Lender = null;
                }
                context.UpdateRange(lentBooks);
            }
            context.Remove(user);
            return Save();
        }

        public User GetUser(int id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public ICollection<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public bool UpdateUser(User user)
        {
            context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return context.Users.Any(u => u.Id == id);
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }
    }
}
