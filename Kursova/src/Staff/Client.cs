using System;
using System.Collections.Generic;

namespace Staff
{
    public class Client : User
    {
        public List<string> BookingHistory { get; set; } = new List<string>();

        public override void DisplayMenu()
        {
            Console.WriteLine($"\n--- Меню пасажира: {Name} {LastName} {ThirdName}---");
            Console.WriteLine("1. Пошук та купівля квитків");
            Console.WriteLine("2. Мої заброньовані квитки (Історія)");
            Console.WriteLine("3. Онлайн-реєстрація на рейс");
            Console.WriteLine("0. Вихід з акаунта");
        }
    }
}