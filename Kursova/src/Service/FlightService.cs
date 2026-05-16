using System;
using System.Collections.Generic;
using Transport;
using Data;

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

            _messageService.ShowMessage("\n=========================================================================");
            _messageService.ShowMessage("                           ІНФОРМАЦІЙНЕ ТАБЛО РЕЙСІВ                     ");
            _messageService.ShowMessage("=========================================================================");
            _messageService.ShowMessage($"{"№ Рейсу",-10} | {"Звідки",-12} | {"Куди",-12} | {"Дата та час вильоту",-16} | {"Ціна (базова)",-10}");
            _messageService.ShowMessage("-------------------------------------------------------------------------");

            foreach (var flight in flights)
            {
                _messageService.ShowMessage(
                    $"{flight.FlightId,-10} | " +
                    $"{flight.Origin,-12} | " +
                    $"{flight.Destination,-12} | " +
                    $"{flight.DepartureTime:dd.MM.yyyy HH:mm} | " +
                    $"{flight.BasePrice,10:C2}"
                );
            }
            _messageService.ShowMessage("=========================================================================\n");
        }

        public void CreateFlightFromConsole(List<Flight> flights, List<AirPlane> airplanes)
        {
            _messageService.ShowMessage("\n--- СТВОРЕННЯ НОВОГО РЕЙСУ ---");

            _messageService.ShowMessage("Введіть номер рейсу (наприклад, PS711):");
            string number = Console.ReadLine() ?? "Unknown";

            _messageService.ShowMessage("Введіть місто вильоту (Origin, наприклад, Kyiv):");
            string origin = Console.ReadLine() ?? "Kyiv";

            _messageService.ShowMessage("Введіть місто прильоту (Destination, наприклад, Paris):");
            string destination = Console.ReadLine() ?? "Unknown";

            // 1. Введення часу вильоту
            _messageService.ShowMessage("Введіть дату та час вильоту (формат РРРР-ММ-ДД ГГ:ХХ, наприклад 2026-05-20 15:30):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime departureTime))
            {
                departureTime = DateTime.Now.AddDays(1);
                _messageService.ShowMessage($"Некоректний формат. Автоматично встановлено: {departureTime}");
            }

            // 2. Введення часу прибуття
            _messageService.ShowMessage("Введіть дату та час ПРИБУТТЯ в місце призначення (формат РРРР-ММ-ДД ГГ:ХХ):");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime arrivalTime))
            {
                arrivalTime = departureTime.AddHours(2); // Якщо помилка, ставимо політ на 2 години автоматично
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
                _messageService.ShowMessage("\n--- ДОСТУПНІ ЛІТАКИ В БАЗІ ---");
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

            _messageService.ShowMessage("Бажаєте призначити ID працівників екіпажу зараз? (д/н):");
            if (Console.ReadLine()?.ToLower() == "д")
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
    }
}