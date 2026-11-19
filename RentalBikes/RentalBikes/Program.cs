using System;
using System.Globalization;
using System.Text;

namespace RentalBikes
{
    internal class Program
    {
        // Умови для табулювання вартості
        const double Tariff = 40;    // грн за годину
        const double Deposit = 300;  // застава, грн

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Довідкові дані для таблиці виручки
            string[] modelNames = { "Міський", "Гірський", "Шосейний", "Електро", "Дитячий" };
            double[] modelTariffs = { 30, 45, 60, 90, 25 };   // грн за годину
            string[] dayNames = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };
            int[] dayHours = { 2, 2, 3, 3, 4, 8, 9 };         // умовна кількість годин прокату за день

            // Робочий масив значень і лічильник заповнених елементів
            double[] values = new double[100];
            int count = 0;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2":
                        {
                            double from = ReadDouble("Початок, годин (0,5–12): ", 0.5, 12);
                            double to = ReadDouble($"Кінець, годин ({from}–12): ", from, 12);
                            double step = ReadDouble("Крок, годин (0,1–12): ", 0.1, 12);
                            Console.WriteLine();
                            ShowCostTable(from, to, step);
                            Pause();
                            break;
                        }
                    case "3":
                        {
                            int rows = ReadInt($"Скільки моделей показати (1–{modelNames.Length}): ", 1, modelNames.Length);
                            int cols = ReadInt($"Скільки днів показати (1–{dayNames.Length}): ", 1, dayNames.Length);
                            ShowRevenueTable(modelNames, modelTariffs, dayNames, dayHours, rows, cols);
                            Pause();
                            break;
                        }
                    case "4":
                        {
                            double newValue = ReadDouble("Нове значення (від -1000000 до 1000000): ", -1000000, 1000000);
                            int before = count;
                            AddValue(values, ref count, newValue, values.Length);
                            Console.WriteLine($"Лічильник у Main: було {before}, стало {count}.");
                            Pause();
                            break;
                        }
                    case "5": ShowValues(values, count); Pause(); break;
                    case "6":
                        {
                            if (count == 0)
                            {
                                Console.WriteLine("Масив порожній: статистики немає.");
                            }
                            else
                            {
                                Statistics(values, count, out double sum, out double average);
                                FindMax(values, count, out double max, out int maxIndex);
                                Console.WriteLine($"Елементів: {count}");
                                Console.WriteLine($"Сума: {sum:F2}");
                                Console.WriteLine($"Середнє: {average:F2}");
                                Console.WriteLine($"Найбільше: {max:F2} (номер {maxIndex + 1})");
                            }
                            Pause();
                            break;
                        }
                    case "7":
                        {
                            int number = ReadInt("Номер елемента для видалення (з таблиці): ", -1000000, 1000000);
                            int before = count;
                            RemoveAt(values, ref count, number - 1);
                            Console.WriteLine($"Лічильник у Main: було {before}, стало {count}.");
                            Pause();
                            break;
                        }
                    case "8": ShowPassingDemo(); Pause(); break;
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
            Console.WriteLine("4 — додати значення");
            Console.WriteLine("5 — показати значення");
            Console.WriteLine("6 — статистика (сума, середнє, найбільше)");
            Console.WriteLine("7 — видалити значення за номером");
            Console.WriteLine("8 — дослід: значення і посилання");
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

        // Дійсне число в межах [min; max] і повторює запит, доки не отримає прийнятне
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine() ?? "";
                text = text.Trim().Replace(',', '.');
                double value;

                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                    || double.IsNaN(value))
                {
                    Console.WriteLine("Помилка: введіть число, наприклад 2,5.");
                }
                else if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                }
                else
                {
                    return value;
                }
            }
        }

        // Ціле число в межах [min; max] і повторює запит, доки не отримає прийнятне
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                int value;

                if (!int.TryParse(text, out value))
                {
                    Console.WriteLine("Помилка: введіть ціле число.");
                }
                else if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                }
                else
                {
                    return value;
                }
            }
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

        // Таблиця модель + день тижня: rows моделей, cols днів, із підсумками
        static void ShowRevenueTable(string[] modelNames, double[] tariffs, string[] dayNames, int[] dayHours, int rows, int cols)
        {
            if (rows <= 0 || cols <= 0)
            {
                Console.WriteLine("Нема чого виводити.");
                return;
            }

            Console.WriteLine("\nУмовна виручка за день, грн");

            // Шапка — до зовнішнього циклу
            Console.Write(new string(' ', 10));
            for (int c = 0; c < cols; c++)
            {
                Console.Write($"{dayNames[c],9}");
            }
            Console.WriteLine($"{"Разом",9}");

            double[] colSums = new double[cols];
            double grandTotal = 0;

            for (int r = 0; r < rows; r++)
            {
                double rowSum = 0; // підсумок рядка — усередині зовнішнього циклу
                Console.Write($"{modelNames[r],-10}");
                for (int c = 0; c < cols; c++)
                {
                    double value = CalculateCost(dayHours[c], tariffs[r]);
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

        // Додає значення в кінець масиву; лічильник змінюється, тому передається через ref
        static void AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count == capacity)
            {
                Console.WriteLine("Місця більше немає.");
                return;
            }

            values[count] = newValue;
            count++;
        }

        // Виводить перші count елементів масиву таблицею з номерами; нічого не питає
        static void ShowValues(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Масив порожній.");
                return;
            }

            Console.WriteLine($"{"№",4}{"Значення",14}");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1,4}{values[i],14:F2}");
            }
        }

        // Знаходить найбільше значення та його індекс; викликати тільки для count > 0
        static void FindMax(double[] values, int count, out double max, out int maxIndex)
        {
            max = 0;
            maxIndex = -1;
            if (count == 0)
            {
                return;
            }

            max = values[0];
            maxIndex = 0;
            for (int i = 1; i < count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    maxIndex = i;
                }
            }
        }

        // Рахує суму та середнє; для порожнього набору середнє дорівнює нулю
        static void Statistics(double[] values, int count, out double sum, out double average)
        {
            sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }

            average = count > 0 ? sum / count : 0;
        }

        // Видаляє елемент із номером index (з нуля), зсуває решту вліво й зменшує лічильник
        static void RemoveAt(double[] values, ref int count, int index)
        {
            if (index < 0 || index >= count)
            {
                Console.WriteLine("Немає елемента з таким номером. Нічого не змінено.");
                return;
            }

            for (int i = index; i < count - 1; i++)
            {
                values[i] = values[i + 1];
            }

            count--;
            values[count] = 0;
        }

        // Намагається змінити int без ref: змінюється лише копія параметра
        static void TryChangeNumber(int number)
        {
            number = number + 100;
            Console.WriteLine($"  Усередині методу number = {number}");
        }

        // Змінює елемент масиву: масив передається посиланням, тому зміна видна ззовні
        static void ChangeFirstElement(double[] numbers)
        {
            numbers[0] = numbers[0] + 100;
            Console.WriteLine($"  Усередині методу numbers[0] = {numbers[0]}");
        }

        // Показує різницю між значимим і посилальним типом
        static void ShowPassingDemo()
        {
            int price = 50;
            double[] prices = { 50, 70, 90 };

            Console.WriteLine($"int price = {price}");
            TryChangeNumber(price);
            Console.WriteLine($"Після TryChangeNumber(price): price = {price}. Оригінал не змінився: метод змінив копію.");
            Console.WriteLine();

            Console.WriteLine($"double[] prices = {{ 50, 70, 90 }}, prices[0] = {prices[0]}");
            ChangeFirstElement(prices);
            Console.WriteLine($"Після ChangeFirstElement(prices): prices[0] = {prices[0]}. Зміна видна: масив передається посиланням.");
        }
    }
}