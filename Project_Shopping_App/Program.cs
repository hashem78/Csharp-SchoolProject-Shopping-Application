﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingApp
{
    class Program
    {
        public static void Menu(User usr)
        {
            bool flag = true;
            while (flag)
            {
                Console.Clear();
                usr.ViewList();
                usr.PrintUserFunctions();
                Console.Write("Please enter your choice(-1 to exit): ");
                string uchoice = Console.ReadLine();
                if (uchoice == "-1")
                    Environment.Exit(Environment.ExitCode);
                usr.HandleChoice(uchoice);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the best shopping app on the planet!");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Enter username: ");
            string uname = Console.ReadLine();
            Console.Write("Enter password: ");
            string upass = Console.ReadLine();
            Menu(Login.LoadUser(uname, upass));

        }
    }
}
