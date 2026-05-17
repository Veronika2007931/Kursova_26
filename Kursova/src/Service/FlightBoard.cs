using Observer;
using Service;
using Transport;

namespace Board
{
    public class FlightBoard : IObserver
    {
        private readonly IMessageService _messageService;
        private readonly List<Flight> _allFlights;

        public FlightBoard(List<Flight> allFlights, IMessageService messageService)
        {
            _allFlights = allFlights;
            _messageService = messageService;
        }

        public void ShowBoard()
        {
            _messageService.ShowMessage($"\n=======================================================================");
            _messageService.ShowMessage($" ІНФОРМАЦІЙНЕ ТАБЛО|  ПОТОЧНИЙ ЧАС: {DateTime.Now:HH:mm:ss}");
            _messageService.ShowMessage($"=======================================================================");
            _messageService.ShowMessage($"{"Рейс",-10} | {"Звідки",-15} | {"Куди",-15} | {"Виліт",-16} | {"Статус",-12}");
            _messageService.ShowMessage($"-----------------------------------------------------------------------");

            foreach (var f in _allFlights)
            {
                _messageService.ShowMessage($"{f.FlightId,-10} | {f.Origin,-15} | {f.Destination,-15} | {f.DepartureTime:dd.MM HH:mm} | {f.Status,-12}");
            }
            _messageService.ShowMessage($"=======================================================================");
        }


        public void Update(string flightId, string status, string origin, string destination)
        {
            _messageService.ShowMessage($"\nОПЕРАТИВНЕ СПОВІЩЕННЯ| 🕒 {DateTime.Now:HH:mm}");
            _messageService.ShowMessage($"⚡ Увага! Рейс {flightId} ({origin} -> {destination}) змінив статус на: {status.ToUpper()} ");


            ShowBoard();
        }
    }
}
