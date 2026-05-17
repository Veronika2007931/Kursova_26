namespace Observer
{
    public interface IObserver
    {
        void Update(string flightId, string status, string origin, string destination);
    }
}