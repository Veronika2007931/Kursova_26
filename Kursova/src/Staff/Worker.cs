using System;

namespace Staff
{
    public class Worker : User
    {
        public string Position { get; set; } = string.Empty;

        public override void DisplayMenu()
        {
            Console.WriteLine($"\n--- Меню співробітника: {Name} ({Position}) ---");
            Console.WriteLine("1. Переглянути мої призначення на рейси");
            Console.WriteLine("0. Вихід з акаунта");
        }
    }
}