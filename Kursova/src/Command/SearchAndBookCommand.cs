using System.Collections.Generic;
using Command;
using Transport;
using Service;
using Staff;

namespace Command
{
    public class SearchAndBookCommand : ICommand
    {
        private readonly FlightService _flightService;
        private readonly BookingService _bookingService;
        private readonly List<Flight> _flights;
        private readonly Client _currentClient;

        public string Description => "Пошук рейсів та купівля квитка";

        public SearchAndBookCommand(FlightService flightService, BookingService bookingService, List<Flight> flights, Client client)
        {
            _flightService = flightService;
            _bookingService = bookingService;
            _flights = flights;
            _currentClient = client;
        }

        public void Execute()
        {
            List<Flight> searchResult = _flightService.SearchFlight(_flights);

            if (searchResult != null && searchResult.Count > 0)
            {
                Flight flightToBook = searchResult[0];
                _bookingService.BookTicket(flightToBook, _currentClient);
            }
        }
    }
}