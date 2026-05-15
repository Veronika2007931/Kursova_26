using Staff;

namespace Transport
{
    public class Flight
    {
        public string FlightId { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public AirPlane Aircraft { get; set; } = new();
        public decimal BasePrice { get; set; }

        public List<string> StaffIds { get; set; } = new();
        public Dictionary<string, string> OccupiedSeats { get; set; } = new();
    }
}