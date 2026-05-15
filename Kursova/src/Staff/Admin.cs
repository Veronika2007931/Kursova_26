namespace Staff
{
    public class Admin : User
    {
        public override void DisplayMenu()
        {
            System.Console.WriteLine("\nАкаунт адміністратора");
            System.Console.WriteLine("1. Створити новий рейс");
            System.Console.WriteLine("2. Редагувати існуючий рейс");
            System.Console.WriteLine("3. Скасувати рейс");
            System.Console.WriteLine("4. Управління персоналом");
            System.Console.WriteLine("5. Переглянути статистику бронювань");
            System.Console.WriteLine("0. Вихід");
        }
    }
}