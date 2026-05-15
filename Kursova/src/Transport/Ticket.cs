using System;
using System.Diagnostics.Contracts;

namespace Transport
{
    public class Ticket
    {
        public string TicketId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        public string FlightNumber { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerLastName { get; set; } = string.Empty;
        public string PassengerThirdName { get; set; } = string.Empty;
        public string PassengerId { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public bool IsCheckedIn { get; set; } = false;

        public void ShowTicketInfo()
        {
            Console.WriteLine($"Квиток №{TicketId} | Рейс: {FlightNumber}");
            Console.WriteLine($"Пасажир: {PassengerName} {PassengerLastName} {PassengerThirdName}");
            Console.WriteLine($"Місце вильоту: {From}");
            Console.WriteLine($"Місце прибуття: {To}");
            Console.WriteLine($"Час та дата вильоту: {DepartureDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Місце: {SeatNumber} | До сплати: {TotalPrice} грн");
            Console.WriteLine(IsCheckedIn ? "[Зареєстровано]" : "[Очікує реєстрації]");
        }
    }
}
