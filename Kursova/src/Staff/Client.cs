using System;
using System.Collections.Generic;

namespace Staff
{
    public class Client : User
    {
        public List<string> BookingHistory { get; set; } = new List<string>();

        public override bool DisplayMenu(
            List<Transport.Flight> flights,
            List<User> users,
            List<Transport.AirPlane> airplanes,
            Data.AirportDatabaseFacade db,
            Service.FlightService flightService,
            Service.BookingService bookingService,
            Service.EmployeeService employeeService
        )
        {
            Console.WriteLine($"\n--- Меню пасажира: {Name} {LastName} {ThirdName}---");
            Console.WriteLine("1. Пошук та купівля квитків");
            Console.WriteLine("2. Мої заброньовані квитки (Історія)");
            Console.WriteLine("3. Повернути квиток");
            Console.WriteLine("4. Переглянути інформаційне табло аеропорту");
            Console.WriteLine("0. Вихід з акаунта");

            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    Command.ICommand searchAndBook = new Command.SearchAndBookCommand(flightService, bookingService, flights, this);
                    searchAndBook.Execute();

                    db.ExportAll(flights, users, airplanes);
                    break;
                case "2":
                    Console.WriteLine("\n--- Твоя історія бронювань ---");
                    if (BookingHistory.Count == 0)
                    {
                        Console.WriteLine("У вас ще немає куплених квитків.");
                    }
                    else
                    {
                        foreach (var ticketId in BookingHistory)
                        {
                            Console.WriteLine($"- Квиток №: {ticketId}");
                        }
                    }
                    break;
                case "3":
                    bookingService.CancelBooking(flights, this);
                    db.ExportAll(flights, users, airplanes); // Оновлюємо файли бази даних
                    break;
                case "4":
                    flightService.ShowFlightBoard(flights);
                    break;
                case "0":
                    Console.WriteLine("Вихід з акаунта пасажира...");
                    return false;

                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }

            return true;
        }
    }
}
