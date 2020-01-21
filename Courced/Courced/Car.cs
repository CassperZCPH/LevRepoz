using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Courced
{
    class Car : IDisposable
    {
        private string markka = "";
        private string model = "";
        private string stateNum = "";
        private string color = "";

        public Car(string nameOwner)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "mark:", markka },
                { "model:", model },
                { "num:", stateNum },
                { "color:", color }
            };
            using (StreamReader infoOfCar = new StreamReader(nameOwner + "/infoOfCarOffender.txt", Encoding.Default))
            {
                for (int i = 1; i <= 4; i++)
                {
                    string[] line = infoOfCar.ReadLine().Split();
                    for (int j = 1; j < line.Length; j++)
                    {
                        data[line[0]] += line[j] + " ";
                    }
                }
            }
            markka = data["mark:"];
            model = data["model:"];
            stateNum = data["num:"];
            color = data["color:"];
        }

        public void PrintInfo()
        {
            Console.WriteLine(color + markka + model + "\t" + stateNum);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
