using Staff;
using System.Collections.Generic;
using Observer;

namespace Transport
{
    public class Flight : ISubject
    {
        public string FlightId { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public AirPlane Aircraft { get; set; } = new();
        public decimal BasePrice { get; set; }

        public List<string> StaffIds { get; set; } = new();
        public Dictionary<string, string> OccupiedSeats { get; set; } = new();

        private string _status = "On Time";

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyObservers();
                }
            }
        }

        private readonly List<IObserver> _observers = new();

        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(FlightId, Status, Origin, Destination);
            }
        }
    }
}