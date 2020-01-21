using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Courced
{
    class Offence : IDisposable
    {
        private DateTime datetime = new DateTime();
        private string wardPlace = "";
        private int wardNumber = 0;
        private string type = "";
        private double sumFine = 0.0;
        public static double totalSum = 0.0;
        public int number = 0;

        public Offence(string numOffence)
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>
            {
                { "datetime:", datetime },
                { "wardPlace:", wardPlace },
                { "wardNum:", wardNumber },
                { "type:", type },
                { "sum:", sumFine }
            };
            using (StreamReader infoOfOffence = new StreamReader(numOffence, Encoding.Default))
            {
                for (int i = 1; i <= 5; i++)
                {
                    string[] line = infoOfOffence.ReadLine().Split();
                    string boof = "";
                    for (int j = 1; j < line.Length; j++)
                    {
                        boof += line[j] + " ";
                    }
                    data[line[0]] = Convert.ChangeType(boof, data[line[0]].GetType());
                }
            }
            number = int.Parse(new FileInfo(numOffence).Name.Replace(".txt", ""));
            datetime = data["datetime:"];
            wardPlace = data["wardPlace:"];
            wardNumber = data["wardNum:"];
            type = data["type:"];
            sumFine = data["sum:"];
            if (DateTime.Now.Subtract(datetime).Days < 15)
            {
                sumFine /= 2;
            }
            totalSum += sumFine;
            if (DateTime.Now.AddMonths(-2) > datetime)
            {
                string path = numOffence.Replace(number.ToString(), (number + 1).ToString());
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
                {
                    sw.WriteLine("datetime: " + datetime.AddMonths(2).ToString().Replace(".", "/"));
                    sw.WriteLine("wardPlace: " + wardPlace);
                    sw.WriteLine("wardNum: " + wardNumber);
                    sw.WriteLine("type: Неуплата штрафа №" + number);
                    sw.WriteLine("sum: " + (sumFine * 2));
                }
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine(new String('-', 60));
            Console.WriteLine("Нарушение №" + number);
            Console.WriteLine(type + "\n");
            Console.WriteLine("Дата и время: " + datetime.ToString("F"));
            Console.WriteLine("Место нарушения: " + wardPlace);
            Console.WriteLine("Камера №" + wardNumber);
            Console.WriteLine("Сумма штрафа: " + sumFine + "руб.");
            Console.WriteLine(new String('-', 60));
        }

        public void Pay(string nameOwner)
        {
            Console.WriteLine("Cумма штрафа: " + sumFine + " руб.");
            InputOutputController.Check(sumFine);
            int index = nameOwner.LastIndexOf(" ", nameOwner.Length - 2);
            string path = "offenders/" + nameOwner.Remove(index, nameOwner.Length - index) + "/" + number + ".txt";
            File.Delete(path);
        }

        public static void PayAll(string nameOwner)
        {
            Console.WriteLine("Общая сумма: " + totalSum + " руб.");
            InputOutputController.Check(totalSum);
            int index = nameOwner.LastIndexOf(" ", nameOwner.Length - 2);
            string path = "offenders/" + nameOwner.Remove(index, nameOwner.Length - index);
            Directory.Delete(path, true);
        }

        public void Dispose()
        {
            totalSum -= sumFine;
            GC.SuppressFinalize(this);
        }
    }
}
