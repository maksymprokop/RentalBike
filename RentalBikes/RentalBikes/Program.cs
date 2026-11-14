using System;
using System.Text;
 
namespace BikeRental
{
    internal class Program
    {
        // Довідкові дані для таблиці (фіксовані масиви, колекцій немає)
        static readonly string[] ModelNames = { "Міський", "Гірський", "Шосейний", "Електро", "Дитячий" };
        static readonly double[] ModelTariffs = { 30, 45, 60, 90, 25 };   // грн за годину
        static readonly string[] DayNames = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
        static readonly int[] DayHours = { 2, 2, 3, 3, 4, 8, 9 };         // умовна кількість годин прокату за день
 
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
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        ShowRevenueTable();
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
 
        // Табулювання: вартість прокату залежно від кількості годин
        static void ShowCostTable()
        {
            double from = 0.5;
            double to = 6.0;
            double step = 0.5;
            double tariff = 40;    // грн за годину
            double deposit = 300;  // застава, грн
 
            Console.WriteLine("Вартість прокату (тариф 40 грн/год, застава 300 грн)");
            Console.WriteLine($"{"Годин",8}{"Вартість",12}{"З заставою",14}");
            
            for (double h = from; h <= to + 1e-9; h += step)
            {
                double cost = h * tariff;
                double withDeposit = cost + deposit;
                Console.WriteLine($"{h,8:F1}{cost,12:F2}{withDeposit,14:F2}");
            }
        }
 
        // Зчитує кількість у межах 1..max; при помилці повертає 0
        static int ReadCount(string prompt, int max)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();
            int value;
            if (!int.TryParse(text, out value) || value < 1 || value > max)
            {
                Console.WriteLine($"Помилка: потрібне ціле число від 1 до {max}.");
                return 0;
            }
            return value;
        }
 
        // Таблиця з двома входами: рядки — моделі, стовпці — дні тижня
        static void ShowRevenueTable()
        {
            int rows = ReadCount($"Скільки моделей показати (1–{ModelNames.Length}): ", ModelNames.Length);
            if (rows == 0) return;
            int cols = ReadCount($"Скільки днів показати (1–{DayNames.Length}): ", DayNames.Length);
            if (cols == 0) return;
 
            Console.WriteLine("\nУмовна виручка за день, грн");
 
            // Шапка — до зовнішнього циклу
            Console.Write(new string(' ', 10));
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{DayNames[c],9}");
            }
            Console.WriteLine($"{"Разом",9}");
 
            double[] colSums = new double[cols];
            double grandTotal = 0;
 
            for (int r = 0; r < rows; r++)
            {
                double rowSum = 0; // підсумок рядка — усередині зовнішнього циклу
                Console.Write($"{ModelNames[r],-10}");
                for (int c = 0; c < cols; c++)
                {
                    double value = ModelTariffs[r] * DayHours[c];
                    rowSum += value;
                    colSums[c] += value;
                    Console.Write($"{value,9:F0}");
                }
                grandTotal += rowSum;
                Console.WriteLine($"{rowSum,9:F0}");
            }
 
            // Рядок підсумків під таблицею
            Console.Write($"{"Разом",-10}");
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{colSums[c],9:F0}");
            }
            Console.WriteLine($"{grandTotal,9:F0}");
        }
    }
}