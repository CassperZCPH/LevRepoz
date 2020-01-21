using System;
using System.Collections.Generic;

namespace Courced
{
    static class Performer
    {
        private delegate void Action();
        public static Owner owner;

        public static void SelectAction()
        {
            Dictionary<string, Action> methods = new Dictionary<string, Action>
            {
                {"1", owner.PrintFullInfo },
                {"2", owner.PrintInfoOffences },
                {"3", owner.PayFine },
                {"4", owner.PayAllFine },
                {"5", Escape}
            };
            string select = null;
            while (select != "5")
            {
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1) Показать полную информацию о пользователе");
                Console.WriteLine("2) Показать полную информацию о штрафах");
                Console.WriteLine("3) Оплатить конкретный штраф");
                Console.WriteLine("4) Оплатить все штрафы");
                Console.WriteLine("5) Выход");
                InputOutputController.Check(out select);
                methods[select]();
            }
        }

        private static void Escape()
        {
            owner.Dispose();
            Console.Clear();
            Console.WriteLine("Начать программу заново? Д/Н");
            Console.Write("Выбор: ");
            if (Console.ReadKey().KeyChar.ToString().ToLower() == "д")
            {
                Console.Clear();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
