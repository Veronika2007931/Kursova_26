using System.Text.Json;
using Transport;
using Staff;
using Service;

namespace Data
{
    public class AirportDatabaseFacade
    {
        private readonly IMassageService _messageService;
        private const string FlightsPath = "flighs.json";
        private const string UsersPath = "users.json";
        public AirportDatabaseFacade(IMassageService messageService)
        {
            _messageService = messageService;
        }

        public void ExportAll(List<Flight> flights, List<User> users)
        {
            string flighsJson = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FlightsPath, flighsJson);

            string usersJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersPath, usersJson);

            _messageService.ShowMessage("Дані успішно експортовані.");

        }

        public (List<Flight>, List<User>) ImportAll()
        {
            var flights = File.Exists(FlightsPath)
            ? JsonSerializer.Deserialize<List<Flight>>(File.ReadAllText(FlightsPath))
            : new List<Flight>();

            var users = File.Exists(UsersPath)
            ? JsonSerializer.Deserialize<List<User>>(File.ReadAllText(UsersPath))
            : new List<User>();

            return (flights ?? new(), users ?? new());
        }
    }


}