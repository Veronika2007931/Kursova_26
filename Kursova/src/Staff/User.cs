using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Staff
{
    [JsonDerivedType(typeof(Admin), typeDiscriminator: "Admin")]
    [JsonDerivedType(typeof(Client), typeDiscriminator: "Client")]
    [JsonDerivedType(typeof(Worker), typeDiscriminator: "Worker")]
    public abstract class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 7).ToUpper();

        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ThirdName { get; set; } = string.Empty;
        public string Role { get; set; } = "";

        public abstract bool DisplayMenu(
            List<Transport.Flight> flights,
            List<User> users,
            List<Transport.AirPlane> airplanes,
            Data.AirportDatabaseFacade db,
            Service.FlightService flightService,
            Service.BookingService bookingService,
            Service.EmployeeService employeeService
        );
    }
}