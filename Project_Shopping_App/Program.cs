using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingApp
{
    class Program
    {
        public static void AddProduct(User usr, Product P)
        {
            usr.AddProduct(P);
        }
        public static void Menu(User usr)
        {
            //View products in store
            bool flag = true;
            while (flag)
            {
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
            Console.Write("Enter username: ");
            string uname = Console.ReadLine();
            Console.Write("Enter password: ");
            string upass = Console.ReadLine();
            var userC = Login.LoadUser(uname, upass);
            if (userC.a != null)
            {
                Admin adm = userC.a;
                Menu(adm);
            }
            else if (userC.c != null)
            {
                Customer customer = userC.c;
                Menu(customer);
            }
        }
    }
}
