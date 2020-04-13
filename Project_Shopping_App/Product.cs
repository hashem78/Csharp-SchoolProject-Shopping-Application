using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        }

        public Product(string id, string name, string category, double price, double quantity)
        {
            _product_id = id;
            _product_name = name;
            _product_category = category;
            _product_price = price;
            _product_quantity = quantity;
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
            string space = "    ";
            Console.WriteLine(ProductId + space + ProductName + space + ProductQuantity + space + ProductPrice + space + ProductQuantity);

        }
    }
    [Serializable]
    abstract class StoreList
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
            }else
            {
                _list = new Hashtable();
            }
        }
        public static void AddProduct(Product P)
        {
            _list.Add(P.ProductId, P);
            SaveStoreList();
        }
        public static void DeleteProduct(string id)
        {
            if (_list.ContainsKey(id))
            {
                _list.Remove(id);
                SaveStoreList();
            }
            else
            {
                throw new ArgumentNullException();
            }
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
        public void AddProduct(Product P)
        {
            _list.Add(P.ProductId, P);
        }
        public void DeleteProduct(string id)
        {
            if (_list.ContainsKey(id))
                _list.Remove(id);
            else
                throw new ArgumentNullException();
        }
        public Product GetProduct(string id)
        {
            if (_list.ContainsKey(id))
                return (Product)_list[id];
            return null;
        }
    }

}