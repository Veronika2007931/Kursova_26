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

        public Ticket? BookingTicket(Flight flight, Client client, string seatName, double baggageWeight, IBaggagePricingStrategy baggageStrategy)
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
            _messageService.ShowMessage($"Квиток успішно заброньовано! Місце: {seatName}. Ціна з багажем: {finalPrice} грн.");

            return newTicket;
        }
    }
}