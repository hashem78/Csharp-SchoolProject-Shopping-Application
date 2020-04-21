using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace shoppingApp
{
    sealed class Login
    {
        public static bool CheckUserExists(string username)
        {
            if (File.Exists(@"data\users\" + username + ".dat"))
                return true;
            return false;
        }
        public static User LoadUser(string username, string password)
        {
            if (CheckUserExists(username))
            {
                FileStream fs = new FileStream(@"data\users\" + username + ".dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                User U = bf.Deserialize(fs) as User;
                fs.Close();
                if (U.Password == password)
                {
                    return U;
                }
            }
            return null;
        }
        public static Customer GetCustomer(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Customer C = bf.Deserialize(fs) as Customer;
                fs.Close();
                if (C != null)
                    return C;
            }
            return null;
        }
        public static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the best shopping app on the planet!");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Exit");
            Console.Write("Enter your choice(1-2): ");
            string uchoice = Console.ReadLine();
            if (uchoice == "1")
            {
                Console.Clear();

                Console.Write("Enter username: ");
                string uname = Console.ReadLine();
                Console.Write("Enter password: ");
                string upass = Console.ReadLine();
                User user = LoadUser(uname, upass);
                if (user == null)
                {
                    Console.WriteLine("Failed to login, user {0} doesn\'t exist or password entered isn\'t correct!", uname);
                    return;
                }
                bool flag = true;
                while (flag)
                {
                    Console.Clear();
                    user.ViewList();
                    user.PrintUserFunctions();
                    Console.Write("Please enter your choice: ");
                    uchoice = Console.ReadLine();
                    user.HandleChoice(uchoice);
                }
            }
            else if (uchoice == "2")
            {
                Environment.Exit(Environment.ExitCode);
                return;

            }
            else
            {
                Console.WriteLine("Wrong choice!");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                Menu();
            }
        }
    }
}


