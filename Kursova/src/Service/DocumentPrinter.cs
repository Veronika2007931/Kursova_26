using System;

namespace Service
{

    public abstract class DocumentPrinter
    {

        public void Print()
        {
            PrintHeader();
            PrintBody();
            PrintFooter();
        }

        private void PrintHeader()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine($"✈️ ЦЕНТРАЛЬНА СИСТЕМА АЕРОПОРТУ | ДАТА: {DateTime.Now:dd.MM.yyyy}");
            Console.WriteLine("=======================================================");
        }

        protected abstract void PrintBody();

        private void PrintFooter()
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("        Бажаємо вам приємного та безпечного польоту!   ");
            Console.WriteLine("=======================================================\n");
        }
    }
}