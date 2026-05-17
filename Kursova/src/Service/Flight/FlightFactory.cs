using Transport;

namespace Service
{
    public class FlightFactory
    {
        public Flight CreateFlight(string number, string origin, string dest, DateTime departure, DateTime arrival, AirPlane plane, decimal price)
        {
            var flight = new Flight
            {
                FlightId = number,
                Origin = origin,
                Destination = dest,
                DepartureTime = departure,
                ArrivalTime = arrival,
                Aircraft = plane,
                BasePrice = price
            };

            for (int r = 0; r < plane.Rows; r++)
            {
                for (int c = 0; c < plane.Columns; c++)
                {
                    string seatName = plane.GetSeatName(r, c);
                    flight.OccupiedSeats.Add(seatName, "Empty");
                }
            }

            return flight;
        }
    }
}