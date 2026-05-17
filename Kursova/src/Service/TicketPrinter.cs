using Transport;

namespace Service
{
    public class TicketPrinter : DocumentPrinter
    {
        private readonly Ticket _ticket;

        public TicketPrinter(Ticket ticket)
        {
            _ticket = ticket;
        }

        protected override void PrintBody()
        {
            Console.WriteLine($"   ПОСАДКОВИЙ ТАЛОН №: {_ticket.TicketId}");
            Console.WriteLine($"   Рейс:               {_ticket.FlightNumber} ({_ticket.From} -> {_ticket.To})");
            Console.WriteLine($"   Пасажир:            {_ticket.PassengerLastName} {_ticket.PassengerName}");
            Console.WriteLine($"   Дата вильоту:       {_ticket.DepartureDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"   Місце в літаку:     {_ticket.SeatNumber}");
            Console.WriteLine($"   Разом до сплати:    {_ticket.TotalPrice} грн");
            Console.WriteLine(_ticket.IsCheckedIn ? "   Статус:             [ЗАРЕЄСТРОВАНО]" : "   Статус:             [ОЧІКУЄ РЕЄСТРАЦІЇ]");
        }
    }
}