using System;

namespace Courced
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Здраствуйте, вас приветсвует программа оплаты штрафов.");
            while (true)
            {
                string nameOwner = Entrance.NumEntry();
                Entrance.CodeEntry();
                Console.Clear();
                Owner owner = new Owner(nameOwner);
                owner.PrintShortInfo();
                Performer.owner = owner;
                Performer.SelectAction();
                Console.WriteLine("good");
                Console.ReadKey();
            }
        }
    }
}
