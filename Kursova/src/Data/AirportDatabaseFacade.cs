using System.Text.Json;
using Transport;
using Staff;
using Service;

namespace Data
{
    public class AirportDatabaseFacade
    {
        private readonly IMessageService _messageService;
        private const string FlightsPath = "flighs.json";
        private const string UsersPath = "users.json";
        private const string AirplanesPath = "airplanes.json";
        public AirportDatabaseFacade(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void ExportAll(List<Flight> flights, List<User> users, List<AirPlane> airplanes)
        {
            string flighsJson = JsonSerializer.Serialize(flights, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FlightsPath, flighsJson);

            string usersJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersPath, usersJson);

            string airplanesJson = JsonSerializer.Serialize(airplanes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(AirplanesPath, airplanesJson);


            _messageService.ShowMessage("Дані успішно експортовані.");
        }

        public (List<Flight>, List<User>, List<AirPlane> airplanes) ImportAll()
        {
            var flights = File.Exists(FlightsPath)
            ? JsonSerializer.Deserialize<List<Flight>>(File.ReadAllText(FlightsPath))
            : new List<Flight>();

            var users = File.Exists(UsersPath)
            ? JsonSerializer.Deserialize<List<User>>(File.ReadAllText(UsersPath))
            : new List<User>();

            var airplanes = File.Exists("airplanes.json")
            ? JsonSerializer.Deserialize<List<AirPlane>>(File.ReadAllText("airplanes.json"))
            : new List<AirPlane>();

            return (flights ?? new(), users ?? new(), airplanes ?? new());


        }
    }


}