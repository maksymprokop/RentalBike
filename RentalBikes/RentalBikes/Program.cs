using System;
using System.Text;
 
namespace BikeRental
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;
 
            while (running)
            {
                Console.WriteLine("=== Прокат велосипедів ===");
                Console.WriteLine("1 — довідка");
                Console.WriteLine("2 — вартість прокату (табулювання)");
                Console.WriteLine("3 — виручка за моделями й днями тижня");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();
                Console.WriteLine();
 
                switch (choice)
                {
                    case "1":
                        ShowReference();
                        Pause();
                        break;
                    case "2":
                        Console.WriteLine("Цей пункт буде реалізовано пізніше.");
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("Цей пункт буде реалізовано пізніше.");
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }
 
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }
 
        // Довідка: тема, сутність, поля з типами
        static void ShowReference()
        {
            Console.WriteLine("Тема: Прокат велосипедів.");
            Console.WriteLine("Головна сутність: Видача (8 полів):");
            Console.WriteLine("  int      — номер");
            Console.WriteLine("  int      — код моделі велосипеда (з довідника)");
            Console.WriteLine("  string   — клієнт");
            Console.WriteLine("  DateTime — дата й час видачі");
            Console.WriteLine("  int      — кількість годин");
            Console.WriteLine("  double   — тариф за годину");
            Console.WriteLine("  double   — застава");
            Console.WriteLine("  bool     — чи повернено");
            Console.WriteLine("Довідник: Модель велосипеда (код, назва, чи активний).");
            Console.WriteLine("Сутність: Користувач (з'явиться в роботі 14).");
        }
    }
}