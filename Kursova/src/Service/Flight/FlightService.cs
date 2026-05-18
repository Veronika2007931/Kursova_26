using System;
using System.Collections.Generic;
using Transport;
using Data;

using Observer;

namespace Service
{
    public class FlightService
    {
        private readonly IMessageService _messageService;
        private readonly FlightFactory _flightFactory;

        public FlightService(IMessageService messageService)
        {
            _messageService = messageService;
            _flightFactory = new FlightFactory();
        }

        public void ShowFlightBoard(List<Flight> flights)
        {
            if (flights == null || flights.Count == 0)
            {
                _messageService.ShowMessage("\n--- ІНФОРМАЦІЙНЕ ТАБЛО ПОРОЖНЄ ---");
                _messageService.ShowMessage("Наразі немає запланованих рейсів.");
                return;
            }
            DateTime currentTime = DateTime.Now;


            List<Flight> activeFlights = new List<Flight>();
            foreach (var flight in flights)
            {

                if (flight.DepartureTime.AddHours(2) < currentTime)
                {
                    continue;
                }


                activeFlights.Add(flight);
            }


            if (activeFlights.Count == 0)
            {
                _messageService.ShowMessage("\n--- Інфомаційте табло порожнє ---");
                _messageService.ShowMessage("Усі заплановані рейси вже застаріли та вилетіли.");
                return;
            }

            _messageService.ShowMessage("\n=================================================================================");
            _messageService.ShowMessage($"                           ІНФОРМАЦІЙНЕ ТАБЛО РЕЙСІВ  [{currentTime:dd.MM HH:mm}]");
            _messageService.ShowMessage("=================================================================================");
            _messageService.ShowMessage($"{"№ Рейсу",-10} | {"Звідки",-12} | {"Куди",-12} | {"Дата та час вильоту",-19} | {"Статус",-12} | {"Ціна",-10}");
            _messageService.ShowMessage("---------------------------------------------------------------------------------");

            foreach (var flight in activeFlights)
            {
                string statusDisplay = flight.Status;
                if (flight.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) ||
                    flight.Status.Equals("Скасовано", StringComparison.OrdinalIgnoreCase))
                {
                    statusDisplay = "СКАСОВАНО";
                }
                else if (flight.DepartureTime < currentTime)
                {
                    statusDisplay = "🛫 ВИЛЕТІВ";
                }

                _messageService.ShowMessage(
                    $"{flight.FlightId,-10} | " +
                    $"{flight.Origin,-12} | " +
                    $"{flight.Destination,-12} | " +
                    $"{flight.DepartureTime:dd.MM.yyyy HH:mm,-19} | " +
                    $"{statusDisplay,-12} | " +
                    $"{flight.BasePrice,10:C2}"
                );
            }
            _messageService.ShowMessage("=================================================================================\n");
        }

        public void CreateFlight(List<Flight> flights, List<AirPlane> airplanes)
        {
            _messageService.ShowMessage("\n--- Ствооення нового рейсу ---");

            _messageService.ShowMessage("Введіть номер рейсу (наприклад, PS711):");
            string number = Console.ReadLine() ?? "Unknown";

            _messageService.ShowMessage("Введіть місто вильоту (Origin, наприклад, Kyiv):");
            string origin = Console.ReadLine() ?? "Kyiv";

            _messageService.ShowMessage("Введіть місто призначення (Destination, наприклад, Paris):");
            string destination = Console.ReadLine() ?? "Unknown";


            _messageService.ShowMessage("Введіть дату та час вильоту (формат РРРР-ММ-ДД ГГ:ХХ, наприклад 2026-05-20 15:30):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime departureTime))
            {
                departureTime = DateTime.Now.AddDays(1);
                _messageService.ShowMessage($"Некоректний формат. Автоматично встановлено: {departureTime}");
            }


            _messageService.ShowMessage("Введіть дату та час прибуття в місце призначення (формат РРРР-ММ-ДД ГГ:ХХ):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime arrivalTime))
            {
                arrivalTime = departureTime.AddHours(2);
                _messageService.ShowMessage($"Некоректний формат. Автоматично встановлено прибуття: {arrivalTime}");
            }

            _messageService.ShowMessage("Введіть базову ціну квитка (грн):");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                price = 1000.0m;
            }

            if (airplanes == null || airplanes.Count == 0)
            {
                _messageService.ShowMessage("Помилка: У базі даних немає жодного літака! Створення рейсу неможливе.");
                return;
            }

            AirPlane chosenPlane = null!;
            while (true)
            {
                _messageService.ShowMessage("\n--- Доступні літаки в базі ---");
                foreach (var plane in airplanes)
                {
                    _messageService.ShowMessage($"ID: [{plane.Id}] | Модель: {plane.Model} (Місць: {plane.Rows}x{plane.Columns})");
                }

                _messageService.ShowMessage("\nВведіть ID літака для призначення на рейс:");
                string inputId = Console.ReadLine()?.Trim().ToUpper() ?? "";


                chosenPlane = airplanes.Find(p => p.Id.ToUpper() == inputId)!;

                if (chosenPlane != null)
                {
                    break;
                }
                _messageService.ShowMessage($"Помилка: Літака з ID '{inputId}' не існує в базі даних. Спробуйте ще раз.");
            }

            Flight newFlight = _flightFactory.CreateFlight(number, origin, destination, departureTime, arrivalTime, chosenPlane, price);

            _messageService.ShowMessage("Бажаєте призначити ID працівників екіпажу зараз? (т/н):");
            if (Console.ReadLine()?.ToLower() == "т")
            {
                _messageService.ShowMessage("Введіть ID працівників через кому (наприклад, WRK01, WRK02):");
                string staffInput = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(staffInput))
                {
                    foreach (var id in staffInput.Split(','))
                    {
                        newFlight.StaffIds.Add(id.Trim().ToUpper());
                    }
                }
            }

            flights.Add(newFlight);
            _messageService.ShowMessage($"\nРейс {number} успішно створено та прив'язано до літака {chosenPlane.Model}!");

        }

        public List<Flight> SearchFlight(List<Flight> flights)
        {
            _messageService.ShowMessage("\n=== Пошук рейсів ===");

            _messageService.ShowMessage("Введіть місто вильоту:");
            string origin = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть місто призначення:");
            string destination = Console.ReadLine()?.Trim() ?? "";

            _messageService.ShowMessage("Введіть дату вильоту (наприклад 2026-05-20) або натисніть Enter, щоб показати на всі дати:");
            string dateInput = Console.ReadLine()?.Trim() ?? "";

            var query = flights.Where(f =>
            f.Origin.Equals(origin, StringComparison.OrdinalIgnoreCase) &&
            f.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime searchDate))
            {
                query = query.Where(f => f.DepartureTime.Date == searchDate.Date);
            }

            List<Flight> foundFlights = query.ToList();
            if (foundFlights.Count == 0)
            {
                _messageService.ShowMessage("\n На жаль, за вашим запитом рейсів не знайдено.");
                return new List<Flight>();
            }
            _messageService.ShowMessage($"\n✅ Знайдено підходящих рейсів: {foundFlights.Count}");
            _messageService.ShowMessage("----------------------------------------------------------------------");
            foreach (var flight in foundFlights)
            {
                _messageService.ShowMessage(
                    $"Рейс: {flight.FlightId} | {flight.Origin} -> {flight.Destination} | " +
                    $"Виліт: {flight.DepartureTime:dd.MM.yyyy HH:mm} | Прибуття: {flight.ArrivalTime:dd.MM.yyyy HH:mm} | " +
                    $"Ціна від: {flight.BasePrice} грн"
                );
            }
            _messageService.ShowMessage("----------------------------------------------------------------------");


            _messageService.ShowMessage("\nВведіть номер рейсу, який ви хочете обрати для купівлі (або '0' для скасування):");
            string selectedNumber = Console.ReadLine()?.Trim().ToUpper() ?? "";

            if (selectedNumber == "0")
            {
                _messageService.ShowMessage("Операцію скасовано.");
                return new List<Flight>();
            }

            Flight choosenFlight = foundFlights.Find(f => f.FlightId.ToUpper() == selectedNumber)!;

            if (choosenFlight == null)
            {
                _messageService.ShowMessage(" Помилка: Ви ввели номер рейсу, якого немає у списку знайдених.");
                return new List<Flight>();
            }
            _messageService.ShowMessage($"\nВи обрали рейс {choosenFlight.FlightId}. Переходимо до вибору місця...");
            return new List<Flight> { choosenFlight };
        }

        public void ChangeFlightStatus(List<Flight> flights)
        {
            _messageService.ShowMessage("\n--- Зміна статусу рейсу---");
            _messageService.ShowMessage("Введіть номер рейсу для зміни статусу:");
            string id = Console.ReadLine()?.Trim().ToUpper() ?? "";


            Flight flight = flights.Find(f => f.FlightId.ToUpper() == id)!;
            if (flight == null)
            {
                _messageService.ShowMessage("Рейс не знайдено.");
                return;
            }


            var flightBoard = new Board.FlightBoard(flights, _messageService);
            flight.RegisterObserver(flightBoard);

            _messageService.ShowMessage($"Поточний статус рейсу {flight.FlightId}: {flight.Status}");
            _messageService.ShowMessage("Оберіть новий статус (1 - Delayed, 2 - Boarding, 3 - Departed):");


            while (true)
            {
                string choice = Console.ReadLine()?.Trim() ?? "";
                if (choice == "1") { flight.Status = "Delayed"; break; }
                else if (choice == "2") { flight.Status = "Boarding"; break; }
                else if (choice == "3") { flight.Status = "Departed"; break; }


                _messageService.ShowMessage("Помилка: Оберіть цифру від 1 до 3:");
            }


            flight.RemoveObserver(flightBoard);
        }

        public void CanсelFlight(List<Flight> flights)
        {
            _messageService.ShowMessage("\n--- Скасування рейсу ---");
            _messageService.ShowMessage("Введіть номер рейсу для скасування (наприклад, PS711):");
            string id = Console.ReadLine()?.Trim().ToUpper() ?? "";

            Flight flight = flights.Find(f => f.FlightId.ToUpper() == id)!;
            if (flight == null)
            {
                _messageService.ShowMessage(" Рейс не знайдено.");
                return;
            }

            _messageService.ShowMessage($"Ви впевнені, що хочете скасувати рейс {flight.FlightId} ({flight.Origin} -> {flight.Destination})? (д/н):");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirm == "д" || confirm == "y" || confirm == "yes")
            {
                var flightBoard = new Board.FlightBoard(flights, _messageService);
                flight.RegisterObserver(flightBoard);
                flight.Status = "Cancelled";

                flight.RemoveObserver(flightBoard);
                _messageService.ShowMessage($"Рейс {flight.FlightId} успішно скасовано.");
            }
            else
            {
                _messageService.ShowMessage("Скасування відхилено.");
            }
        }

        public void CreateAirPlane(List<AirPlane> airplanes)
        {
            _messageService.ShowMessage("\n--- Створення нового літака ---");

            string id = "";
            while (true)
            {
                _messageService.ShowMessage("Введіть унікальний бортовий ID літака (наприклад, B737, AN225):");
                id = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (string.IsNullOrEmpty(id))
                {
                    _messageService.ShowMessage("ID літака не може бути порожнім.");
                    continue;
                }

                // ЗАХИСТ ВІД ДУБЛІКАТІВ: Перевіряємо, чи існує вже такий літак у базі
                if (airplanes.Exists(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    _messageService.ShowMessage($"Літак з ID [{id}] вже існує в базі даних! Створення скасовано.");
                    return; // Одразу перериваємо метод, дублікат не створиться!
                }
                break;
            }

            _messageService.ShowMessage("Введіть модель літака:");
            string model = Console.ReadLine()?.Trim() ?? "Unknown Plane";

            int rows = 0;
            while (true)
            {
                _messageService.ShowMessage("Введіть кількість рядів сидінь (позитивне число, наприклад, 20):");
                if (int.TryParse(Console.ReadLine(), out rows) && rows > 0)
                {
                    break;
                }
                _messageService.ShowMessage("Помилка: Кількість рядів має бути більшою за 0.");
            }

            int columns = 0;
            while (true)
            {
                _messageService.ShowMessage("Введіть кількість місць у ряду (число від 1 до 10, наприклад, 6):");
                if (int.TryParse(Console.ReadLine(), out columns) && columns > 0 && columns <= 10)
                {
                    break;
                }
                _messageService.ShowMessage("Помилка: Кількість місць у ряду має бути від 1 до 10.");
            }


            AirPlane newPlane = new AirPlane
            {
                Id = id,
                Model = model,
                Rows = rows,
                Columns = columns
            };

            airplanes.Add(newPlane);
            _messageService.ShowMessage($"\nЛітак [{model}] успішно створено! ID: {newPlane.Id} (Всього місць: {newPlane.TotalSeats})");
        }

        public void DeleteAirPlane(List<AirPlane> airplanes, List<Flight> flights)
        {
            _messageService.ShowMessage("\n--- ВИДАЛЕННЯ ЛІТАКА З БАЗИ ---");

            if (airplanes.Count == 0)
            {
                _messageService.ShowMessage("У базі даних немає жодного літака.");
                return;
            }

            _messageService.ShowMessage("Введіть ID літака, який потрібно видалити:");
            string id = Console.ReadLine()?.Trim().ToUpper() ?? "";

            AirPlane planeToDelete = airplanes.Find(p => p.Id.ToUpper() == id)!;

            if (planeToDelete == null)
            {
                _messageService.ShowMessage($"Літака з ID [{id}] не знайдено.");
                return;
            }


            bool isAssignedToFlight = flights.Exists(f => f.Aircraft != null && f.Aircraft.Id.ToUpper() == id);
            if (isAssignedToFlight)
            {
                _messageService.ShowMessage($"Літак [{id}] призначений на один із рейсів! Видалення неможливе, спочатку скасуйте або видаліть цей рейс.");
                return;
            }

            _messageService.ShowMessage($"Ви впевнені, що хочете назавжди видалити літак [{planeToDelete.Model}] з ID [{id}]? (т/н):");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirm == "т" || confirm == "y" || confirm == "yes")
            {
                airplanes.Remove(planeToDelete);
                _messageService.ShowMessage($"Літак [{id}] успішно видалено з системи.");
            }
            else
            {
                _messageService.ShowMessage("Видалення скасовано.");
            }
        }
    }
}