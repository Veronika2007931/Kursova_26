using Transport;

namespace Service
{
    public class FlightFactory
    {
        public Flight CreateFlight(string number, string dest, AirPlane plane, decimal price)
        {
            var flight = new Flight
            {
                FlightId = number,
                Destination = dest,
                Aircraft = plane,
                BasePrice = price,
                DepartureTime = DateTime.Now.AddDays(1)
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