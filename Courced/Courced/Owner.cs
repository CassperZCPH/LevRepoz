using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Courced
{
    class Owner : IDisposable, IReadbleObject, IWritableObject
    {
        private string FIO = "";
        private long telephone = 0;
        private string registrationAddress = "";
        private int[] passportData = new int[3];
        private int[] numberDL = new int[3]; // Driver's License
        private DateTime dateOfGettingDL = new DateTime();
        private string driverCategory = "";
        private Car car;
        private int quantityOffence;
        private Offence[] offences;
        private string path;

        private Owner(ILoadManager man)
        {
            FIO = man.ReadLine().Split(':')[1];

            telephone = Convert.ToInt64(man.ReadLine().Split(':')[1]);
            registrationAddress = man.ReadLine().Split(':')[1];
            string[] boof = man.ReadLine().Split(' ');
            for (int k = 0; k < 3; k++)
            {
                passportData[k] = int.Parse(boof[k + 1]);
            }
            boof = man.ReadLine().Split(' ');
            for (int k = 0; k < 3; k++)
            {
                numberDL[k] = int.Parse(boof[k + 1]);
            }
            dateOfGettingDL = Convert.ToDateTime(man.ReadLine().Split(' ')[1]);
            driverCategory = man.ReadLine().Split(':')[1];
            /*
            car = man.Read(new Car.Loader("infoofCar"));
            string path = man.Path;
            
            quantityOffence = Directory.GetFiles(path).Length - 2;
            */
        }

        public Owner(string name)
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>
            {
                { "FIO:", FIO },
                { "phone:", telephone },
                { "regAddr:", registrationAddress },
                { "pasData:", passportData },
                { "numDL:", numberDL },
                { "dateOfGetDL:", dateOfGettingDL },
                { "drivCateg:", driverCategory }
            };
            path = "offenders/" + name;
            using (StreamReader infoOfOwner = new StreamReader(path + "/infoOfOffender.txt", Encoding.Default))
            {
                for (int i = 1; i <= 7; i++)
                {
                    string[] line = infoOfOwner.ReadLine().Split();
                    if (data[line[0]].GetType().ToString() == "System.Int32[]")
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            data[line[0]][k] = int.Parse(line[k + 1]);
                        }
                        continue;
                    }
                    string boof = "";
                    for (int j = 1; j < line.Length; j++)
                    {
                        boof += line[j] + " ";
                    }
                    data[line[0]] = Convert.ChangeType(boof, data[line[0]].GetType());
                }
            }
            FIO = data["FIO:"];
            telephone = data["phone:"];
            registrationAddress = data["regAddr:"];
            passportData = data["pasData:"];
            numberDL = data["numDL:"];
            dateOfGettingDL = data["dateOfGetDL:"];
            driverCategory = data["drivCateg:"];
            car = new Car(path);
            string[] filesOffences = Directory.GetFiles(path);
            quantityOffence = filesOffences.Length - 2;
            Array.Resize(ref offences, quantityOffence);
            for (int i = 0; i < quantityOffence; i++)
            {
                offences[i] = new Offence(filesOffences[i]);
                filesOffences = Directory.GetFiles(path);
                quantityOffence = filesOffences.Length - 2;
                Array.Resize(ref offences, quantityOffence);
            }
        }

        public void PrintShortInfo()
        {
            Console.WriteLine(new String('-', 58) + "\n" + new String('-', 58));
            Console.WriteLine(FIO);
            car.PrintInfo();
            Console.WriteLine("Количество нарушений: " + quantityOffence);
            Console.WriteLine(new String('-', 58) + "\n" + new String('-', 58));
        }

        public void PrintFullInfo()
        {
            Console.Clear();
            Console.WriteLine(new String('-', 70) + "\n" + new String('-', 70));
            Console.WriteLine(FIO + $" {telephone:+# (###) ###-##-##}");
            Console.Write("Серия номер паспорта:");
            for (int i = 0; i < passportData.Length; i++)
            {
                Console.Write(" " + passportData[i]);
            }
            Console.WriteLine("\nАдресс регистрации: " + registrationAddress + "\n");
            Console.Write("Номер водительского удоставерения:");
            for (int i = 0; i < numberDL.Length; i++)
            {
                Console.Write(" " + numberDL[i]);
            }
            Console.WriteLine("  :  Категория: " + driverCategory);
            Console.WriteLine("Дата получения ВУ: " + dateOfGettingDL.ToLongDateString());
            car.PrintInfo();
            Console.WriteLine("Количество нарушений: " + quantityOffence);
            Console.WriteLine(new String('-', 70) + "\n" + new String('-', 70));
        }

        public void PrintInfoOffences()
        {
            Console.Clear();
            foreach (Offence offence in offences)
            {
                offence.PrintInfo();
            }
            Console.WriteLine("Сумма всех штрафов состовляет: " + Offence.totalSum + " руб.");
            Console.WriteLine(new String('-', 50));
        }
        public void PayFine()
        {
            Console.WriteLine("Введите номер штрафа");
            InputOutputController.Check(out int index, offences, out bool trig);
            if (trig)
            {
                return;
            }
            offences[index].Pay(FIO);
            List<Offence> boofList = new List<Offence>();
            foreach (Offence offence in offences)
            {
                boofList.Add(offence);
            }
            boofList.RemoveAt(index);
            offences[index].Dispose();
            for (int i = 0; i < boofList.Count; i++)
            {
                offences[i] = boofList[i];
            }
            Array.Resize(ref offences, boofList.Count);
            if (Directory.GetFiles(path).Length == 2)
            {
                Console.WriteLine("Все штрафы погашенны");
                Console.WriteLine("Завершение программы");
                Directory.Delete(path, true);
                Environment.Exit(0);
            }
            Console.WriteLine("Штраф погашен");
            Console.Clear();
        }

        public void PayAllFine()
        {
            Offence.PayAll(FIO);
            Console.WriteLine("Все штрафы погашенны");
            Console.WriteLine("Завершение программы");
            Environment.Exit(0);
        }

        public void Dispose()
        {
            foreach (Offence offence in offences)
            {
                offence.Dispose();
            }
            car.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Write(ISaveManager man)
        {
            man.WriteLine($"FIO: {FIO}");
            man.WriteLine($"phone: {telephone}");
            man.WriteLine($"regAddr: {registrationAddress}");
            string boof = null;
            foreach (int item in passportData)
            {
                boof += item + " ";
            }
            man.WriteLine($"pasData: {boof}");
            boof = null;
            foreach (int item in numberDL)
            {
                boof += item + " ";
            }
            man.WriteLine($"numDL: {boof}");
            man.WriteLine($"dateOfGetDL: {dateOfGettingDL}");
            man.WriteLine($"drivCateg: {driverCategory}");
        }
        public class Loader : IReadableObjectLoader
        {
            public Loader() { }
            public IReadbleObject Load(ILoadManager man)
            {
                return new Owner(man);
            }
        }
    }
}
