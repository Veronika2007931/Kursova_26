using System.Runtime.CompilerServices;

namespace Staff
{
    public abstract class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public abstract void DisplayMenu();
    }
}