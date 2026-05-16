using Data;
using Service;
using Staff;
using Transport;

IMessageService messageService = new ConsoleMessageService();

var db = new AirportDatabaseFacade(messageService);
var (flights, users, airplanes) = db.ImportAll();
messageService.ShowMessage("Ласкаво просимо до системи обліку аеропорту!");

if (users.Count == 0)
{
    users.Add(new Admin
    {
        Login = "admin",
        Password = "123",
        Name = "Veronika",
        LastName = "Niema"
    });

    db.ExportAll(flights, users, airplanes);
}

var authService = new AuthService();
messageService.ShowMessage("Введіть логін:");
string login = Console.ReadLine() ?? "";

messageService.ShowMessage("Введіть пароль:");
string pass = Console.ReadLine() ?? "";

User? currentUser = authService.Login(users, login, pass);

if (currentUser != null)
{
    messageService.ShowMessage($"Вітаємо, {currentUser.Name}!");
    currentUser.DisplayMenu();

}
else
{
    messageService.ShowMessage("Невірний логін або пароль.");
}



