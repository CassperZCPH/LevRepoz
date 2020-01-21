using System;

namespace Courced
{
    static class InputOutputController
    {
        public static void Offside(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Offside()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Вы превысили количество попыток! Завершенние программы!");
            Environment.Exit(0);
        }

        public static void Check(out string select)
        {
            string[] numbers = new string[5] { "1", "2", "3", "4", "5" };
            for (int i = 1; i <= 3; i++)
            {
                if (i > 1)
                {
                    Console.WriteLine($"Попытка №{i}");
                }
                Console.Write("Выбор: ");
                select = Console.ReadKey().KeyChar.ToString();
                if (Array.IndexOf(numbers, select) != -1)
                {
                    Console.WriteLine();
                    return;
                }
                else
                {
                    Offside("\nОшибка выбора!");
                }
            }
            Offside();
            select = null;
        }

        public static void Check(double totalSum)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i > 1)
                {
                    Console.WriteLine($"Попытка №{i}");
                }
                Console.Write("Введите сумму оплаты: ");
                if (double.TryParse(Console.ReadLine(), out double sum) && sum == totalSum)
                {
                    return;
                }
                else
                {
                    Offside("Ошибка ввода суммы! Введите сумму равную сумме штрафа!");
                }
            }
            Offside();
        }

        public static void Check(out int index, Offence[] offences, out bool trig)
        {
            for (int i = 1; i <= 3; i++)
            {
                if (i > 1)
                {
                    Console.WriteLine($"Попытка №{i}");
                }
                Console.Write("Номер: ");
                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    for (int j = 0; j < offences.Length; j++)
                    {
                        if (num == offences[j].number)
                        {
                            index = j;
                            trig = false;
                            return;
                        }
                    }
                    Console.WriteLine("Нарушения с таким номером нет");
                    index = -1;
                    trig = true;
                    return;
                }
                else
                {
                    Offside("Ошибка ввода номера!");
                }
            }
            Offside();
            index = -1;
            trig = true;
        }
    }
}
