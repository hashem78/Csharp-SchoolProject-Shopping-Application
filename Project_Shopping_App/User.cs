﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace shoppingApp
{
    [Serializable]
    class User
    {
        private string _name;
        private string _password;
        protected string[] _functions;

        public string Name
        {
            set
            {
                _name = value;
            }
            get
            {
                return _name;
            }
        }
        public string Password
        {
            set
            {
                _password = value;
            }
            get
            {
                return _password;
            }
        }

        protected string[] Functions
        {
            set
            {
                _functions = value;
            }
            get
            {
                return _functions;
            }
        }

        public User(string Name, string Password)
        {
            this.Name = Name;
            this.Password = Password;
        }
        public virtual void AddProduct(Product product)
        {

        }
        public virtual void ViewStoreProducts(double threshold)
        {

        }
        public void PrintUserFunctions()
        {
            int idx = 1;
            foreach (string function in _functions)
            {
                Console.WriteLine(idx + "." + function);
                idx++;
            }
        }

        public virtual void HandleChoice(string choice)
        {

        }
        public void SaveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("data\\users\\" + Name + ".dat", FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, this);
            fs.Close();
        }

        public virtual void ViewList()
        {
            Console.Clear();
            string space = "            ";
            Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
            foreach (Product p in StoreList.List.Values)
                p.Print();
            Console.WriteLine();
        }
        protected int isChoiceCorrect(string choice)
        {
            try
            {
                string a = Functions[Convert.ToInt32(choice) - 1];
                return Convert.ToInt32(choice);
            }
            catch
            {
                Console.WriteLine("Wrong choice!");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                return -1;
            }
        }
    }
    [Serializable]
    class Admin : User
    {
        public Admin(string Name, string Password) : base(Name, Password)
        {
            Functions = new string[]
            {
            "Add customer.",
            "Delete customer.",
            "Add product",
            "Delete product",
            "Search for product based on Product_ID.",
            "View all users.",
            "View all products based on threshold.",
            "View all orders for a specific customer.",
            "View all orders between dates."
            };
        }
        public override void AddProduct(Product product)
        {
            StoreList.AddProduct(product);
        }
        public override void ViewStoreProducts(double threshold)
        {
            string space = "            ";
            Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
            foreach (Product product in StoreList.List.Values)
            {
                if (product.ProductQuantity >= threshold)
                    product.Print();
            }
        }
        private void AddCustomer()
        {
            Console.Write("Enter username: ");
            string uname = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            Customer C = new Customer(uname, password);
            C.SaveUser();
        }
        private void DeleteCustomer()
        {
            string[] dir = Directory.GetFiles(@"data\users\", "*.dat");
            int idx = 1;
            while (true)
            {
                Console.Clear();
                foreach (string file in dir)
                {
                    int a = file.LastIndexOf('\\') + 1;
                    string ans = idx + "." + file.Substring(a, file.IndexOf('.') - a);
                    idx++;
                    Console.WriteLine(ans);
                }
                try
                {
                    Console.Write("Enter choice: ");
                    int uchoice = Convert.ToInt32(Console.ReadLine());
                    File.Delete(dir[uchoice - 1]);
                    break;
                }
                catch
                {
                    idx = 1;
                    Console.WriteLine("Wrong selection!");
                }

            }

        }
        private void AddProductToStore()
        {
            Console.Clear();
            string id, name, category;
            double price, quantity;

            Console.Write("Please enter product ID: ");
            id = Console.ReadLine();

            Console.Write("Please enter product name: ");
            name = Console.ReadLine();

            Console.Write("Please enter product category: ");
            category = Console.ReadLine();

            Console.Write("Please enter product price: ");
            price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Please enter product quantity: ");
            quantity = Convert.ToDouble(Console.ReadLine());

            AddProduct(new Product(id, name, category, price, quantity));


        }
        private void DeleteProductFromStore()
        {
            Console.Clear();
            foreach (Product p in StoreList.List.Values)
                p.Print();
            Console.Write("Enter ID of product to remove: ");
            try
            {
                string uchoice = Console.ReadLine();
                StoreList.DeleteProduct(uchoice);
            }catch
            {
                Console.WriteLine("Wrong choice!");
                Console.WriteLine("Press any key to continue!");
                Console.ReadKey();
            }
        }
        public override void HandleChoice(string choice)
        {
            int c = isChoiceCorrect(choice);
            if (c == -1)
                return;
            switch (c)
            {
                case 1:
                    AddCustomer();
                    break;
                case 2:
                    DeleteCustomer();
                    break;
                case 3:
                    AddProductToStore();
                    break;
                case 4:
                    DeleteProductFromStore();
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }

    }
    [Serializable]
    class Customer : User
    {
        private string _address;
        public string Address
        {
            set
            {
                _address = value;
            }
            get
            {
                return _address;
            }
        }
        Basket Basket = new Basket();
        public Customer(string Name, string Password) : base(Name, Password)
        {
            Functions = new string[]
            {
            "View all products.",
            "Search for a product based on Product_Category.",
            "Add product to basket.",
            "Money payment.",
            "Add cash credit."
            };
        }
        public override void AddProduct(Product product)
        {
            Basket.AddProduct(product);
        }
        public override void ViewList()
        {
            string space = "            ";
            Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
            foreach (Product p in Basket.List.Values)
                p.Print();
        }
        public override void ViewStoreProducts(double threshold = 0)
        {
            if (threshold == 0)
            {
                string space = "            ";
                Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
                foreach (Product product in Basket.List.Values)
                    product.Print();
            }
        }
        public override void HandleChoice(string choice)
        {
            int c = isChoiceCorrect(choice);
            if (c == -1)
                return;
            switch (c)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }


    }

}
