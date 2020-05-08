using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;


// Everything in this file is supposed to be written by Jafar

namespace shoppingApp
{
    [Serializable]
    class Product
    {
        string _product_id;
        string _product_name;
        string _product_category;
        double _product_price;
        double _product_quantity;

        public DateTime TimeOfPurchase;

        public string ProductId
        {
            get { return _product_id; }
        }
        public string ProductName
        {
            get { return _product_name; }
        }
        public string ProductCategory
        {
            get { return _product_category; }
        }
        public double ProductPrice
        {
            get { return _product_price; }
        }
        public double ProductQuantity
        {
            get { return _product_quantity; }
            set
            {
                _product_quantity = value;
            }
        }

        public Product(string id, string name, string category, double price, double quantity)
        {
            _product_id = id;
            _product_name = name;
            _product_category = category;
            _product_price = price;
            _product_quantity = quantity;
        }
        public Product Clone()
        {
            Product temp = new Product(_product_id, _product_name, _product_category, _product_price, _product_quantity);
            return temp;
        }
        public override int GetHashCode()
        {
            int hash = 0;
            foreach (char x in ProductId)
                hash += x - '0';
            return hash;
        }
        public void Print()
        {
            Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", ProductId, ProductName, ProductCategory, ProductPrice, ProductQuantity);
        }
    }
    [Serializable]
    sealed class StoreList
    {
        private static Hashtable _list;
        private static string _name = "store_list";

        public static string Name
        {
            get
            {
                return _name;
            }
        }
        public static Hashtable List
        {
            get
            {
                return _list;
            }
        }
        static StoreList()
        {
            if (File.Exists(@"data\" + Name + ".dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(@"data\" + Name + ".dat", FileMode.Open, FileAccess.Read);
                _list = (Hashtable)bf.Deserialize(fs);
                fs.Close();
            }
            else
            {
                _list = new Hashtable();
            }
        }
        public static bool isEmpty()
        {           
            foreach(Product P in _list.Values)
                if (P.ProductQuantity != 0)
                    return false;
            return true;
        }
        public static void ViewStoreList(bool showZero = true)
        {
            Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
            
            foreach (Product p in _list.Values)
                if (p.ProductQuantity == 0)
                {
                    if (showZero)
                    {
                        p.Print();
                    }
                }
                else
                {
                    p.Print();
                }
        }
        public static bool AddProduct(string id,double quantity=0)
        {
            if (quantity < 0)
                return false;
            bool success = false;
            if (GetProduct(id) == null)
            {
                string  name, category;
                double price=0, uquantity;

                Console.Write("Enter product name: ");
                name = Console.ReadLine();

                Console.Write("Enter product category: ");
                category = Console.ReadLine();

                while (true)
                {
                    try
                    {
                        bool aprice = true;//ask for price?
                        if (aprice)
                        {
                            Console.Write("Enter product price: ");
                            price = Convert.ToDouble(Console.ReadLine());
                            if (price < 0)
                                throw new FormatException();
                        }
                        Console.Write("Enter product quantity: ");
                        uquantity = Convert.ToDouble(Console.ReadLine());
                        if (uquantity < 0)
                        {
                            aprice = false;
                            throw new FormatException();
                        }
                        success = true;
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Wrong value entered!");
                    }
                }
                Product p = new Product(id, name, category, price, uquantity);
                _list.Add(id, p);
            }else
            {
                GetProduct(id).ProductQuantity += quantity;
                success = true;
            }
            if (success)
            {
                SaveStoreList();
                return true;
            }
            return false;
        }
        public static bool DeleteProduct(string id,double quantity=0)
        {
            if (quantity < 0)
                return false;
            bool success = false;
            if (GetProduct(id) != null)                
            {
                if (GetProduct(id).ProductQuantity >= quantity)
                {
                    GetProduct(id).ProductQuantity -= quantity;
                    success = true;
                }
                if (GetProduct(id).ProductQuantity == 0)
                {
                    _list.Remove(id);
                    success = true;
                }
            }

            if (success)
            {
                SaveStoreList();
                return true;
            }
            return false;
        }
        public static Product GetProduct(string id)
        {
            if (_list.ContainsKey(id))
                return (Product)_list[id];
            return null;
        }
        public static void SaveStoreList()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(@"data\" + Name + ".dat", FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, _list);
            fs.Close();
        }
    }
    [Serializable]
    class Basket
    {
        private Hashtable _list;

        public Hashtable List
        {
            get
            {
                return _list;
            }
        }
        public Basket()
        {
            _list = new Hashtable();
        }
        public bool isEmpty()
        {
            return _list.Count == 0;
        }
        public void Add(Product P, double quantity=0)
        {
            P.TimeOfPurchase = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
            try
            {
                _list.Add(P.ProductId, P);
                //StoreList.GetProduct(P.ProductId).ProductQuantity -= quantity;
                StoreList.SaveStoreList();
            }
            catch (ArgumentException)
            {
                GetProduct(P.ProductId).ProductQuantity += quantity;
            }

        }
        public bool DeleteProduct(string id,double quantity = 0)
        {
            if (quantity < 0)
                return false;
            if (_list.ContainsKey(id))
            {
                if (quantity == 0)
                {
                    _list.Remove(id);
                    return true;
                }
                else
                {
                    if (GetProduct(id).ProductQuantity - quantity > 0)
                    {
                        GetProduct(id).ProductQuantity -= quantity;
                        StoreList.GetProduct(id).ProductQuantity += quantity;
                    }
                    else if (GetProduct(id).ProductQuantity - quantity == 0)
                    {
                        StoreList.GetProduct(id).ProductQuantity += quantity;
                        DeleteProduct(id);
                    }
                    else
                        return false;
                    return true;
                }
            }
            return false;
        }

        public Product GetProduct(string id)
        {
            if (_list.ContainsKey(id))
                return (Product)_list[id];
            return null;
        }
        public void ViewBasket()
        {
            Console.WriteLine("{0,-25} {1,-25} {2,-25} {3,-10} {4,-10}", "ID", "Name", "Category", "Price", "Quantity");
            foreach (Product p in _list.Values)
                p.Print();
        }
    }

}