
using Transport;
using Data;
using Service;

namespace Staff
{
    public class Worker : User
    {

        public Worker()
        {
            Role = "Worker";
        }


        public override bool DisplayMenu(
            List<Transport.Flight> flights,
            List<User> users,
            List<Transport.AirPlane> airplanes,
            Data.AirportDatabaseFacade db,
            Service.FlightService flightService,
            Service.BookingService bookingService,
            Service.EmployeeService employeeService)
        {
            Console.WriteLine($"\nВітаємо, {Name} {LastName}!");
            Console.WriteLine($"Ваша посада: {Role}.");
            Console.WriteLine("У вас немає доступу до керування комп'ютерною системою аеропорту.");
            Console.WriteLine("Натисніть Enter, щоб вийти з акаунта...");

            Console.ReadLine();
            return false;
        }
    }
}