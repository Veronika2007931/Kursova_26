using Data;
using Service;
using Staff;

IMessageService messageService = new ConsoleMessageService();
var db = new AirportDatabaseFacade(messageService);
var flightService = new FlightService(messageService);
var bookingService = new BookingService(messageService);
var employeeService = new EmployeeService(messageService);


var (flights, users, airplanes) = db.ImportAll();

messageService.ShowMessage("Ласкаво просимо до системи обліку аеропорту!");


if (users.Count == 0)
{
    users.Add(new Admin
    {
        Login = "admin",
        Password = "123",
        Name = "Veronika",
        LastName = "Niema",
        Role = "Admin"
    });

    db.ExportAll(flights, users, airplanes);
}

var authService = new AuthService();
User? currentUser = null;


while (currentUser == null)
{
    messageService.ShowMessage("\n=============================");
    messageService.ShowMessage("1. Увійти в систему");
    messageService.ShowMessage("2. Зареєструватися як новий пасажир");
    messageService.ShowMessage("0. Вийти з програми");
    messageService.ShowMessage("=============================");
    messageService.ShowMessage("Оберіть дію:");

    string startChoice = Console.ReadLine()?.Trim() ?? "";

    if (startChoice == "0")
    {
        messageService.ShowMessage("Програму завершено. До побачення!");
        return;
    }

    if (startChoice == "1")
    {
        messageService.ShowMessage("\nВведіть логін:");
        string login = Console.ReadLine() ?? "";

        messageService.ShowMessage("Введіть пароль:");
        string pass = Console.ReadLine() ?? "";

        currentUser = authService.Login(users, login, pass);

        if (currentUser == null)
        {
            messageService.ShowMessage("Невірний логін або пароль. Спробуйте ще раз.");
        }
    }
    else if (startChoice == "2")
    {
        messageService.ShowMessage("\n--- Реєстрація нового клієнта ---");

        string newLogin = "";
        while (true)
        {
            messageService.ShowMessage("Введіть унікальний логін для акаунта:");
            newLogin = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(newLogin))
            {
                messageService.ShowMessage("Логін не може бути порожнім.");
                continue;
            }
            if (users.Exists(u => u.Login.Equals(newLogin, StringComparison.OrdinalIgnoreCase)))
            {
                messageService.ShowMessage("Цей логін уже зайнятий. Оберіть інший.");
                continue;
            }
            break;
        }

        messageService.ShowMessage("Придумайте пароль:");
        string newPass = Console.ReadLine()?.Trim() ?? "";

        messageService.ShowMessage("Введіть ваше ім'я:");
        string newName = Console.ReadLine()?.Trim() ?? "";

        messageService.ShowMessage("Введіть ваше прізвище:");
        string newLastName = Console.ReadLine()?.Trim() ?? "";

        messageService.ShowMessage("Введіть по-батькові:");
        string newThirdName = Console.ReadLine()?.Trim() ?? "";

        Client newClient = new Client
        {
            Login = newLogin,
            Password = newPass,
            Name = newName,
            LastName = newLastName,
            ThirdName = newThirdName,
            Role = "Client"
        };

        users.Add(newClient);
        db.ExportAll(flights, users, airplanes);

        messageService.ShowMessage($"\nКлієнта [{newName} {newLastName}] успішно зареєстровано!");
        messageService.ShowMessage("Тепер оберіть пункт '1' у меню, щоб увійти під своїм логіном.");
    }
    else
    {
        messageService.ShowMessage("Некоректне введення. Оберіть дію зі списку.");
    }
}

messageService.ShowMessage($"\nАвторизація успішна! Вітаємо, {currentUser.Name}!");


bool isRunning = true;
while (isRunning)
{
    isRunning = currentUser.DisplayMenu(flights, users, airplanes, db, flightService, bookingService, employeeService);
}


