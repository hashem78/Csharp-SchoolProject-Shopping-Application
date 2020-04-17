using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Globalization;

namespace shoppingApp
{
    [Serializable]
    class User
    {
        private string _name;
        private string _password;
        

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
        public User(string Name, string Password)
        {
            this.Name = Name;
            this.Password = Password;
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
        public void SaveUser()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(@"data\users\" + Name + ".dat", FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, this);
            fs.Close();
        }

        public virtual void AddProduct()
        { 

        }

        protected string[] _functions;
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

        public virtual void HandleChoice(string choice)
        {

        }
        public virtual void ViewStoreProducts(double threshold)
        {

        }
        public virtual void ViewList()
        {
            Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-20}", "ID", "Name", "Category", "Price", "Quantity");
            foreach (Product p in StoreList.List.Values)
                p.Print();
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
        public override void ViewStoreProducts(double threshold)
        {
            Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {2,-20}", "ID", "Name", "Category", "Price", "Quantity");
            foreach (Product p in StoreList.List.Values)
            {
                if (p.ProductQuantity >= threshold)
                    p.Print();
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
        public void ViewAllUsers()
        {
            Console.Clear();
            string[] dir = Directory.GetFiles(@"data\users\", "*.dat");
            int idx = 1;
            foreach (string file in dir)
            {
                int a = file.LastIndexOf('\\') + 1;
                string ans = idx + "." + file.Substring(a, file.IndexOf('.') - a);
                idx++;
                Console.WriteLine(ans);
            }
        }
        public override void AddProduct()
        {
            Console.Clear();
            string id;
            Console.Write("Please enter product ID: ");
            id = Console.ReadLine();
            StoreList.AddProduct(id);
        }
        private void DeleteProductFromStore()
        {
            Console.Clear();
            foreach (Product p in StoreList.List.Values)
                p.Print();
            Console.Write("Enter ID of product to remove: ");
            string uchoice = Console.ReadLine();
            if (!StoreList.DeleteProduct(uchoice))
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
                    AddProduct();
                    break;
                case 4:
                    DeleteProductFromStore();
                    break;
                case 5:
                    break;
                case 6:
                    ViewAllUsers();
                    break;
                case 7:
                    try
                    {
                        Console.Clear();
                        Console.Write("Enter threshold: ");
                        double threshold = Convert.ToDouble(Console.ReadLine());
                        ViewStoreProducts(threshold);
                    }
                    catch
                    {
                        Console.WriteLine("Wrong value entered!");
                    }
                    break;
                case 8:
                    ViewAllOrdersForCustomer();
                    break;
                case 9:
                    ViewAllOrdersBetweenDates();
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        private void ViewAllOrdersBetweenDates()
        {
            Console.Clear();
            Console.Write("Enter starting date (dd/mm/yy): ");
            string starting = Console.ReadLine();
            Console.Write("Enter ending date (dd/mm/yy): ");
            string ending = Console.ReadLine();

            if (starting[starting.IndexOf("/") + 1] != '0')
                starting = starting.Insert(starting.IndexOf("/") + 1, "0");
            if (ending[ending.IndexOf("/") + 1] != '0')
                ending = ending.Insert(ending.IndexOf("/") + 1, "0");

            DateTime startingDate;
            DateTime endingDate;

            if (!DateTime.TryParseExact(starting, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startingDate))
            {
                Console.WriteLine("Wrong starting date value entered!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            if (!DateTime.TryParseExact(ending, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endingDate))
            {
                Console.WriteLine("Wrong ending date value entered!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            string[] users = Directory.GetFiles(@"data\users\", "*.dat");
            Console.WriteLine("Order            From                 Date");
            foreach (string s in users)
            {
                Customer c = Login.LoadCustomer(s) as Customer;
                if (c != null)
                    foreach (Product p in c.Basket.List.Values)
                        if (DateTime.Compare(startingDate, p.TimeOfPurchase) <= 0)
                            if (DateTime.Compare(endingDate, p.TimeOfPurchase) >= 0)
                                p.Print();
            }
        }
        private void ViewAllOrdersForCustomer()
        {
            ViewAllUsers();
            Console.Write("Enter customer name: ");
            string name = Console.ReadLine();
            BinaryFormatter bf = new BinaryFormatter();
            Customer temp;
            try
            {
                FileStream fs = new FileStream(@"data\users\" + name + ".dat", FileMode.Open, FileAccess.Read);
                temp = (Customer)bf.Deserialize(fs);
                temp.ViewList();
                fs.Close();
            }
            catch(InvalidCastException)
            {
                Console.WriteLine("Wrong value entered!");
            }
            Console.ReadKey();
            Console.WriteLine("Press any key to continue...");
        }

    }
    [Serializable]
    class Customer : User
    {
        private double _balance;
        private string _address;
        public Basket Basket = new Basket();
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
        public double Balance
        {
            get { return _balance; }
            set
            {
                _balance = value;
            }
        }

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

        public override void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Available products");
            StoreList.ViewStoreList(false);
            Console.WriteLine("Products in basket");
            Basket.ViewBasket();
            Console.Write("Enter product ID to add to basket: ");
            string tempID = Console.ReadLine();
            try
            {
                Product temp = StoreList.GetProduct(tempID).Clone();
                Console.Write("Enter quantity: ");
                double uquan = Convert.ToDouble(Console.ReadLine());
                if (temp.ProductQuantity - uquan >= 0)
                {
                    temp.ProductQuantity = uquan;
                    Basket.Add(temp,uquan);
                    SaveUser();
                }
                else
                {
                    Console.WriteLine("Desired quantity is unavailable!");
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Product doesn\'t exist!");
            }
            catch(FormatException)
            {
                Console.WriteLine("Wrong value entered!");
            }
        }
        public override void ViewList()
        {
            Basket.ViewBasket();
        }
        public override void ViewStoreProducts(double threshold = 0)
        {
            if (threshold == 0)
                StoreList.ViewStoreList(false);
        }
        public void Search()
        {
            Dictionary<int,string> categories = new Dictionary<int, string>();
            int idx = 1;
            foreach (Product p in StoreList.List.Values)
                if (!categories.ContainsValue(p.ProductCategory))
                    categories.Add(idx++, p.ProductCategory);
            foreach (KeyValuePair<int,string> d in categories)
                Console.WriteLine(d.Key +". " + d.Value);

            Console.Write("Enter your choice(1-{0}): ",idx-1);

            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-20}", "ID", "Name", "Category", "Price", "Quantity");
                foreach (Product p in StoreList.List.Values)
                    if(p.ProductCategory == categories[choice])
                    {
                        p.Print();
                    }

            }catch(FormatException)
            {
                Console.WriteLine("Wrong value entered!");
            }
        }

        private bool Pay()
        {
            bool success = true;
            double cost = 0;
            while (true)
            {
                Console.Clear();
                Basket.ViewBasket();
                //Calculate cost of items in Basket
                cost = 0;
                foreach (Product p in Basket.List.Values)
                    cost += p.ProductPrice * p.ProductQuantity;
                if (cost > _balance)
                {
                    Console.Write("Would you like to remove some products from your basket?(Y/N): ");
                    string s = Console.ReadLine().ToUpper();
                    if (s == "Y")
                    {
                        Console.Write("Enter id of product to remove/remove from: ");
                        string id = Console.ReadLine();
                        try
                        {
                            Console.Write("Enter quantity to remove: ");
                            double quan = Convert.ToDouble(Console.ReadLine());

                            if (Basket.DeleteProduct(id, quan))
                            {
                                StoreList.AddProduct(id, quan); // could be used later if store no longer has a certian item
                                if (Basket.isEmpty())
                                {
                                    success = false;
                                    break;
                                }
                            }
                            else throw new FormatException(); // id may be not in basket
                        }catch(FormatException)
                        {
                            Console.WriteLine("Wrong value entered!");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                    else if (s == "N")
                    {
                        Console.Write("Press any key to continue...");
                        success = false;
                        break;
                    }else
                    {
                        Console.WriteLine("Wrong value entered!");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                }else
                {
                    success = true;
                    break;
                }
            } 
            SaveUser();
            if (success)
            {
                _balance -= cost;
                Basket.List.Clear();
                return true;
            }
            return false;
        }

        public override void HandleChoice(string choice)
        {
            int c = isChoiceCorrect(choice);
            if (c == -1)
                return;
            switch (c)
            {
                case 1:
                    ViewStoreProducts();
                    break;
                case 2:
                    Search();
                    break;
                case 3:
                    AddProduct();
                    break;
                case 4:
                    Pay();
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

}
