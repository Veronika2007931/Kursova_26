using Staff;

namespace Service
{
    public class AuthService
    {
        public User? Login(List<User> users, string login, string password)
        {
            return users.Find(u => u.Login == login && u.Password == password);
        }
    }
}