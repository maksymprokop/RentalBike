using System;
using System.Text;

namespace RentalBikes
{
    internal class Program
    {
        // Довідкові дані для таблиці (фіксовані масиви, колекцій немає)
        static readonly string[] ModelNames = { "Міський", "Гірський", "Шосейний", "Електро", "Дитячий" };
        static readonly double[] ModelTariffs = { 30, 45, 60, 90, 25 };   // грн за годину
        static readonly string[] DayNames = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
        static readonly int[] DayHours = { 2, 2, 3, 3, 4, 8, 9 };         // умовна кількість годин прокату за день

        // Умови для табулювання вартості
        const double Tariff = 40;    // грн за годину
        const double Deposit = 300;  // застава, грн

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": RunCostTable(); Pause(); break;
                    case "3": ShowRevenueTable(); Pause(); break;
                    case "0": running = false; break;
                    default: ShowUnknownChoice(); Pause(); break;
                }
            }

            Console.WriteLine("До побачення.");
        }

        // Виводить головне меню й запрошення до вибору
        static void ShowMenu()
        {
            Console.WriteLine("=== Прокат велосипедів ===");
            Console.WriteLine("1 — про програму");
            Console.WriteLine("2 — вартість прокату (табулювання)");
            Console.WriteLine("3 — виручка за моделями й днями тижня");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        // Чекає натискання клавіші й очищає екран
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Виводить тему, сутність і перелік її полів із типами
        static void ShowAbout()
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

        // Повідомляє, що такого пункту меню немає
        static void ShowUnknownChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
        }

        // Повертає вартість прокату: кількість годин, помножена на тариф
        static double CalculateCost(double hours, double tariff)
        {
            return hours * tariff;
        }

        // Показує таблицю вартості для готового діапазону
        static void RunCostTable()
        {
            ShowCostTable(0.5, 6.0, 0.5);
        }

        // Табулювання вартості прокату: від from до to з кроком step
        static void ShowCostTable(double from, double to, double step)
        {
            if (step <= 0)
            {
                Console.WriteLine("Крок має бути додатним.");
                return;
            }

            Console.WriteLine($"Вартість прокату (тариф {Tariff} грн/год, застава {Deposit} грн)");
            Console.WriteLine($"{"Годин",8}{"Вартість",12}{"З заставою",14}");

            // 1e-9 — запас, щоб остання стрічка не зникла через похибку double
            for (double h = from; h <= to + 1e-9; h += step)
            {
                double cost = CalculateCost(h, Tariff);
                Console.WriteLine($"{h,8:F1}{cost,12:F2}{cost + Deposit,14:F2}");
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

        // Таблиця «модель × день тижня»; розміри питає у користувача
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
                    double value = CalculateCost(DayHours[c], ModelTariffs[r]);
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