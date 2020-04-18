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
            Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
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
            if (threshold > 0)
            {
                Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
                foreach (Product p in StoreList.List.Values)
                {
                    if (p.ProductQuantity >= threshold)
                        p.Print();
                }
            }
            else
                Console.WriteLine("Wrong value entered!");
        }
        private void AddCustomer()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            if (Login.CheckUserExists(username))
            { 
                Console.WriteLine("Customer {0} already exists, would you like to overwrite?(Y/N): ",username);
                string uchoice = Console.ReadLine().ToUpper();
                if (uchoice == "N")
                    return;
            }
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            Customer C = new Customer(username, password);
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
                    if (uchoice >= 1 && uchoice <= dir.Length)
                    {
                        if(Login.LoadUser(dir[uchoice-1],Password).GetType() != this.GetType())
                            File.Delete(dir[uchoice - 1]);
                        else
                        {
                            Console.WriteLine("User is an admin... can't delete admins!");
                        }
                    }
                    else
                        throw new FormatException();
                    break;
                }
                catch(FormatException)
                {
                    idx = 1;
                    Console.WriteLine("Wrong selection!");
                }

            }

        }
        public void ViewAllUsers()
        {
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
            string id;
            Console.Write("Please enter product ID: ");
            id = Console.ReadLine();
            StoreList.AddProduct(id);
        }
        private void DeleteProductFromStore()
        {
            Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
            foreach (Product p in StoreList.List.Values)
                p.Print();
            Console.Write("Enter ID of product to remove: ");
            string uchoice = Console.ReadLine();
            double quan = 0;
            if (StoreList.GetProduct(uchoice) != null)
            {
                if (StoreList.GetProduct(uchoice).ProductQuantity != 0)
                {
                    Console.Write("Enter quantity: ");
                    try
                    {
                        quan = Convert.ToDouble(Console.ReadLine());
                        if (quan > StoreList.GetProduct(uchoice).ProductQuantity)
                        {
                            Console.WriteLine("Can\'t remove more than available quantity");
                            throw new FormatException();
                        }
                        if (quan < 0)
                            throw new FormatException();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wrong value entered!");
                        return;
                    }
                }
                if (!StoreList.DeleteProduct(uchoice, quan))
                {
                    Console.WriteLine("Wrong choice!");
                }
            }else
            {
                Console.WriteLine("Product does not exist!");
            }

        }
        private void Search()
        {
            Console.Write("Enter ProductId to look for: ");
            string id = Console.ReadLine();
            if(StoreList.GetProduct(id) !=null)
            {
                Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
                StoreList.GetProduct(id).Print();
            }else
            {
                Console.WriteLine("Poduct with Id ({0}) does not exist!", id);
            }
        }

        public override void HandleChoice(string choice)
        {
            Console.Clear();
            int c = isChoiceCorrect(choice);
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
                    Search();
                    break;
                case 6:
                    ViewAllUsers();
                    break;
                case 7:
                    try
                    {
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
                return;
            }
            if (!DateTime.TryParseExact(ending, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endingDate))
            {
                Console.WriteLine("Wrong ending date value entered!");
                return;
            }
            string[] users = Directory.GetFiles(@"data\users\", "*.dat");
            Console.WriteLine("Order            From                 Date");
            foreach (string s in users)
            {
                Customer c = Login.GetCustomer(s);
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
            string username = Console.ReadLine();
            BinaryFormatter bf = new BinaryFormatter();
            Customer temp = Login.GetCustomer(username);
            if(temp != null)
                temp.ViewList();
            else
            {
                Console.WriteLine("Customer is either an admin, or doesn't exist!");
            }
        }

    }
    [Serializable]
    class Customer : User
    {
        private double _balance = 0;
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
                if (uquan < 0)
                    throw new FormatException();
                if (temp.ProductQuantity - uquan >= 0)
                {
                    temp.ProductQuantity = uquan;
                    Basket.Add(temp, uquan);
                    SaveUser();
                }
                else
                {
                    Console.WriteLine("Desired quantity is unavailable!");
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Product doesn\'t exist!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong value entered!");
            }
        }
        public override void ViewList()
        {
            Basket.ViewBasket();
            Console.WriteLine("Current balance is: {0}", _balance);
        }
        public override void ViewStoreProducts(double threshold = 0)
        {
            if (threshold >= 0)
                StoreList.ViewStoreList(false);
        }
        private void Search()
        {
            Dictionary<int, string> categories = new Dictionary<int, string>();
            int idx = 1;
            foreach (Product p in StoreList.List.Values)
                if (!categories.ContainsValue(p.ProductCategory))
                    categories.Add(idx++, p.ProductCategory);
            foreach (KeyValuePair<int, string> d in categories)
                Console.WriteLine(d.Key + ". " + d.Value);

            Console.Write("Enter your choice(1-{0}): ", idx - 1);

            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
                foreach (Product p in StoreList.List.Values)
                    if (p.ProductCategory == categories[choice])
                    {
                        p.Print();
                    }

            }
            catch (FormatException)
            {
                Console.WriteLine("Wrong value entered!");
            }
        }

        private bool Pay()
        {
            if (!Basket.isEmpty())
            {
                double cost = 0;
                bool success = true;
                while (true)
                {
                    Console.Clear();
                    Basket.ViewBasket();
                    Console.WriteLine("Your balance is: {0} ", _balance);
                    //Calculate cost of items in Basket
                    cost = 0;
                    foreach (Product p in Basket.List.Values)
                        cost += p.ProductPrice * p.ProductQuantity;
                    if (cost > _balance)
                    {
                        Console.WriteLine("Insufficient funds...");
                        Console.Write("Would you like to remove some products from your basket?(Y/N): ");
                        string s = Console.ReadLine().ToUpper();
                        if (s == "Y")
                        {
                            Console.Write("Enter id of product to remove/remove from: ");
                            string id = Console.ReadLine();
                            if(Basket.GetProduct(id) == null)//check if product exists in basket
                            {
                                Console.WriteLine("Item entered is not in basket!");
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                continue;
                            }
                            try
                            {
                                Console.Write("Enter quantity to remove: ");
                                double quan = Convert.ToDouble(Console.ReadLine());
                                if (quan < 0)
                                    throw new FormatException();
                                if (quan != 0)//dont delete if quantity is 0
                                {
                                    if (Basket.DeleteProduct(id, quan))
                                    {
                                        StoreList.AddProduct(id, quan);
                                        if (Basket.isEmpty())
                                        {
                                            success = false;
                                            break;
                                        }
                                    }
                                    else throw new FormatException();
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Wrong value entered!");
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        else if (s == "N")
                        {
                            success = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong value entered!");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        success = true;
                        break;
                    }
                }
                SaveUser();
                if (success)
                {
                    while (true)
                    {
                        Console.Write("Total cost is {0} and your balance is {1}, would you like to procceed with the transaction?(Y/N): ", cost, _balance);
                        string uchoice = Console.ReadLine().ToUpper();
                        if (uchoice == "Y")
                        {
                            _balance -= cost;
                            Basket.List.Clear();
                            Console.WriteLine("Deducted {0}, new balance is {1}.", cost, _balance);
                            SaveUser();
                            break;
                        }
                        else if (uchoice == "N")
                        {
                            Console.WriteLine("Transaction cancelled.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong value entered!");
                        }
                    }
                    return true;
                }
            }
            else
            {
                Console.Clear();
                Basket.ViewBasket();
                Console.WriteLine("Your basket is empty, functionality disabled!");
            }
            return false;
        }

        private void AddBalance()
        {
            try
            {
                Console.WriteLine("Current balance is: {0}", _balance);
                Console.Write("Enter amount to deposit: ");
                double amount = Convert.ToDouble(Console.ReadLine());
                if (amount < 0)
                    throw new FormatException();
                Console.WriteLine("Successfully deposited {0} to your account.", amount);
                _balance += amount;
                Console.WriteLine("New balance is: {0}", _balance);
                SaveUser();
            }
            catch (FormatException)
            {
                Console.Write("Wrong value entered!");
            }
        }

        public override void HandleChoice(string choice)
        {
            Console.Clear();
            int c = isChoiceCorrect(choice);
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
                case 5:
                    AddBalance();
                    break;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

}
