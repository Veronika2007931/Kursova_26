namespace Staff
{
    public class Admin : User
    {
        public override bool DisplayMenu(
        List<Transport.Flight> flights,
        List<User> users,
        List<Transport.AirPlane> airplanes,
        Data.AirportDatabaseFacade db,
        Service.FlightService flightService,
        Service.BookingService bookingService,
        Service.EmployeeService employeeService)
        {
            Console.WriteLine("\nАкаунт адміністратора");
            Console.WriteLine("1. Створити новий рейс");
            Console.WriteLine("2. Редагувати існуючий рейс");
            Console.WriteLine("3. Скасувати рейс");
            Console.WriteLine("4. Управління персоналом");
            Console.WriteLine("5. Додати новий літак в базу");
            Console.WriteLine("6. Видалити літак з бази");
            Console.WriteLine("0. Вихід");

            string choice = Console.ReadLine()?.Trim() ?? "";
            switch (choice)
            {
                case "1":
                    flightService.CreateFlight(flights, airplanes);
                    db.ExportAll(flights, users, airplanes);
                    break;
                case "2":
                    flightService.ChangeFlightStatus(flights);
                    db.ExportAll(flights, users, airplanes);
                    break;
                case "3":
                    flightService.CanсelFlight(flights);
                    db.ExportAll(flights, users, airplanes);
                    break;

                case "4":
                    employeeService.ManageStaff(users);
                    db.ExportAll(flights, users, airplanes);
                    break;
                case "5":
                    flightService.CreateAirPlane(airplanes);
                    db.ExportAll(flights, users, airplanes);
                    break;
                case "6":
                    flightService.DeleteAirPlane(airplanes, flights);
                    db.ExportAll(flights, users, airplanes);
                    break;
                case "0":
                    Console.WriteLine("Вихід з акаунта адміністратора...");
                    return false;

                default:
                    Console.WriteLine(" Невірний вибір. Спробуйте ще раз.");
                    break;
            }

            return true;


        }
    }
}
