using Staff;

namespace Service
{
    public static class UserFactory
    {
        public static User CreateUser(string role, string login, string password, string name, string lastName, string thirdName)
        {
            if (role.Equals("Admin", System.StringComparison.OrdinalIgnoreCase))
            {
                return new Admin { Login = login, Password = password, Name = name, LastName = lastName, ThirdName = thirdName, Role = "Admin" };
            }

            return new Worker { Login = login, Password = password, Name = name, LastName = lastName, ThirdName = thirdName, Role = role };
        }
    }
}