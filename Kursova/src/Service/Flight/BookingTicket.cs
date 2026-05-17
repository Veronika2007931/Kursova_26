using Transport;
using Staff;

using System.Security.Cryptography.X509Certificates;

namespace Service
{
    public class BookingService
    {
        private readonly IMessageService _messageService;

        public BookingService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public Ticket? CreateTicket(Flight flight, Client client, string seatName, double baggageWeight, IBaggagePricingStrategy baggageStrategy)
        {
            if (!flight.OccupiedSeats.ContainsKey(seatName) || flight.OccupiedSeats[seatName] != "Empty")
            {
                _messageService.ShowMessage("Помилка: Це місце вже зайняте або не існує.");
                return null;
            }

            decimal baggagePrice = baggageStrategy.CalculatePrice(baggageWeight);
            decimal finalPrice = flight.BasePrice + baggagePrice;

            Ticket newTicket = new Ticket
            {
                FlightNumber = flight.FlightId,
                PassengerName = client.Name,
                PassengerLastName = client.LastName,
                PassengerThirdName = client.ThirdName,
                PassengerId = client.Id,
                From = flight.Origin,
                To = flight.Destination,
                DepartureDate = flight.DepartureTime,
                SeatNumber = seatName,
                TotalPrice = finalPrice,
                IsCheckedIn = false
            };

            flight.OccupiedSeats[seatName] = client.Id;
            client.BookingHistory.Add(newTicket.TicketId);
            _messageService.ShowMessage("Генеруємо офіційний документ квитка...");
            DocumentPrinter printer = new TicketPrinter(newTicket);
            printer.Print();

            return newTicket;
        }

        public void BookTicket(Flight flight, Client client)
        {
            _messageService.ShowMessage($"\n=== Оформлання квитка на рейс {flight.FlightId} ===");
            AirPlane plane = flight.Aircraft;

            _messageService.ShowMessage($"\n--- Схема місць у літаку ---");
            _messageService.ShowMessage("Зайняті місця позначені [ X ], вільні — назвою місця.\n");

            for (int r = 0; r < plane.Rows; r++)
            {
                string rowText = $"Ряд {r + 1:D2}: ";
                for (int c = 0; c < plane.Columns; c++)
                {
                    string seat = plane.GetSeatName(r, c);
                    if (flight.OccupiedSeats.ContainsKey(seat) || flight.OccupiedSeats[seat] != "Empty")
                    {
                        rowText += "[X]";
                    }
                    else
                    {
                        rowText += $"[{seat}]";
                    }
                }
                _messageService.ShowMessage(rowText);
            }
            _messageService.ShowMessage("-------------------------------------------------------");

            string seatName = "";
            while (true)
            {
                _messageService.ShowMessage("\nВведіть назву вільного місця (наприклад, 1A або 2B):");
                seatName = Console.ReadLine()?.Trim().ToUpper() ?? "";

                var indices = plane.GetSeatIndices(seatName);
                if (indices == null)
                {
                    _messageService.ShowMessage("Такого місця не існує в літаку.");
                    continue;
                }
                if (flight.OccupiedSeats.ContainsKey(seatName) && flight.OccupiedSeats[seatName] != "Empty")
                {
                    _messageService.ShowMessage("Це місце вже зайняте.");
                    continue;
                }
                break;
            }
            double baggageWeight = 0;
            while (true)
            {
                _messageService.ShowMessage("\nВведіть вагу вашого багажу в кг (якщо немає — введіть 0):");
                string weightInput = Console.ReadLine()?.Trim() ?? "";

                if (double.TryParse(weightInput, out baggageWeight) && baggageWeight >= 0)
                {
                    break; // Введено коректне число, виходимо з циклу
                }

                _messageService.ShowMessage("Вага багажу повинна бути позитивним числом. Спробуйте ще раз.");
            }

            IBaggagePricingStrategy baggageStrategy;
            while (true)
            {
                _messageService.ShowMessage("\nОберіть тариф вашого квитка:");
                _messageService.ShowMessage("1. Економ тариф");
                _messageService.ShowMessage("2. Бізнес тариф");
                string strategyChoice = Console.ReadLine()?.Trim() ?? "";

                if (strategyChoice == "1")
                {
                    baggageStrategy = new EconomyBaggage();
                    break;
                }
                if (strategyChoice == "2")
                {
                    baggageStrategy = new BusinessBaggage();
                    break;
                }

                _messageService.ShowMessage("Помилка: Оберіть варіант '1' або '2'.");
            }

            Ticket? newTicket = CreateTicket(flight, client, seatName, baggageWeight, baggageStrategy);

            if (newTicket != null)
            {
                _messageService.ShowMessage("\nПриємного польоту!");
            }
        }

        public void CancelBooking(List<Flight> flights, User currentClient)
        {
            _messageService.ShowMessage("\n--- Повернення квитка ---");


            var clientTickets = new List<(Flight flight, string seatNumber, string ticketId)>();

            foreach (var flight in flights)
            {
                foreach (var seat in flight.OccupiedSeats)
                {

                    if (seat.Value == currentClient.Id)
                    {
                        string tId = $"{flight.FlightId}-{seat.Key}";
                        clientTickets.Add((flight, seat.Key, tId));
                    }
                }
            }

            if (clientTickets.Count == 0)
            {
                _messageService.ShowMessage("У вас немає активних заброньованих квитків.");
                return;
            }


            _messageService.ShowMessage("Ваші активні квитки:");
            for (int i = 0; i < clientTickets.Count; i++)
            {
                var item = clientTickets[i];
                _messageService.ShowMessage($"{i + 1}. Квиток ID: [{item.ticketId}] | Рейс: {item.flight.FlightId} ({item.flight.Origin} -> {item.flight.Destination}) | Місце: {item.seatNumber}");
            }

            _messageService.ShowMessage("\nОберіть номер квитка для повернення (або 0 для скасування):");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > clientTickets.Count)
            {
                _messageService.ShowMessage("Некоректний вибір.");
                return;
            }

            if (choice == 0)
            {
                _messageService.ShowMessage("Операцію скасовано.");
                return;
            }

            // Отримуємо обраний квиток
            var selected = clientTickets[choice - 1];

            _messageService.ShowMessage($"Ви впевнені, що хочете повернути квиток на рейс {selected.flight.FlightId}, місце {selected.seatNumber}? (т/н):");
            string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (confirm == "т" || confirm == "y" || confirm == "yes")
            {

                if (selected.flight.OccupiedSeats.ContainsKey(selected.seatNumber))
                {
                    selected.flight.OccupiedSeats.Remove(selected.seatNumber);
                    _messageService.ShowMessage($"\nКвиток [{selected.ticketId}] успішно анульовано!");
                    _messageService.ShowMessage($"Гроші повернуто на рахунок. Місце [{selected.seatNumber}] знову вільне для бронювання.");
                }
            }
            else
            {
                _messageService.ShowMessage("Повернення скасовано.");
            }
        }


    }
}