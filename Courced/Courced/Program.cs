using System;
using System.Collections.Generic;

namespace Courced
{
    class Program
    {
        static void Main(string[] args)
        {
            string nameOwner = Entrance.NumEntry();
            Owner s = new Owner(nameOwner);
            //
            SaveManager saver = new SaveManager("test.txt");
            saver.WriteObject(s);
            //
            LoadManager loader = new LoadManager("test.txt");
            List<Owner> sList = new List<Owner>();
            loader.BeginRead();
            while (loader.IsLoading)
                sList.Add(loader.Read(new Owner.Loader()) as Owner);
            loader.EndRead();
            //
            foreach (Owner st in sList)
                Console.WriteLine(st);
            Console.ReadKey();
            /*
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
            */
        }
    }
}
