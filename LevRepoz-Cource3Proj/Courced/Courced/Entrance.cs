using System;
using System.IO;
using System.Text;

namespace Courced
{
    static class Entrance
    {
        public static string NumEntry()
        {
            Console.WriteLine("Для авторизации введите свой номер телефона в формате (+7 9## ### ## ##)");
            for (int i = 1; i <= 3; i++)
            {
                if (i > 1)
                {
                    Console.WriteLine($"Попытка №{i}");
                }
                Console.Write("Номер: +");
                if (long.TryParse(Console.ReadLine().Replace(" ", ""), out long num) && num >= 79001111111 && num <= 79999999999)
                {
                    bool trig = ExistPhone(num, out string dirName);
                    if (trig)
                    {
                        Console.WriteLine("Вы хотите начать сначала? Д/Н");
                        Console.Write("Выбор: ");
                        if (Console.ReadKey().KeyChar.ToString().ToLower() == "д")
                        {
                            Console.Clear();
                            dirName = NumEntry();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    return dirName;
                }
                else
                {
                    InputOutputController.Offside("Невернный формат ввода!");
                }
            }
            InputOutputController.Offside();
            return null;
        }

        private static bool ExistPhone(long phone, out string dirName)
        {
            string[] dirs = Directory.GetDirectories("offenders");
            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    if (new FileInfo(file).Name == "infoOfOffender.txt")
                    {
                        using (StreamReader sr = new StreamReader(file, Encoding.Default))
                        {
                            string[] line = sr.ReadLine().Split();
                            dirName = line[1] + " " + line[2];
                            line = sr.ReadLine().Split();
                            long.TryParse(line[1], out long num);
                            if (phone == num)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Такого телефона нет в базе. У вас нет текущих нарушений.");
            dirName = null;
            return true;
        }

        private static int SendMessage()
        {
            using (StreamWriter sr = new StreamWriter("phone.txt", false))
            {
                int code = new Random().Next(1234, 9876);
                sr.Write(code);
                return code;
            }
        }

        public static void CodeEntry()
        {
            int trueCode = SendMessage();
            Console.WriteLine("На ваш телефон был выслан код, введите его для авторизации.");
            for (int i = 1; i <= 3; i++)
            {
                if (i > 1)
                {
                    Console.WriteLine($"Попытка №{i}");
                }
                Console.Write("Код: ");
                if (int.TryParse(Console.ReadLine(), out int code) && trueCode == code)
                {
                    Console.WriteLine("Код введён верно.");
                    return;
                }
                else
                {
                    InputOutputController.Offside("Неверный код!");
                }
            }
            InputOutputController.Offside();
        }
    }
}
