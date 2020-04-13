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
    class ProductList
    {
        public static Hashtable StoreProducts;
        //public static HashSet<Product> StoreProducts;
        private HashSet<Product> Products;
        static ProductList()
        {
            if(File.Exists(@"data\pickled_store_products.dat"))
            {
                FileStream fs = new FileStream(@"data\pickled_store_products.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                StoreProducts = (Hashtable)bf.Deserialize(fs);
                fs.Close();
            }
            else
            {
                StoreProducts = new Hashtable();
            }

        }
        public ProductList()
        {
            Products = new HashSet<Product>();
        }
        public static void AddStoreProduct(Product P)
        {
            StoreProducts.Add(StoreProducts.Count+1,P);
            SaveProductList();
        }
        public static void DeleteStoreProduct(int k)
        {
            if (StoreProducts.ContainsKey(k))
                StoreProducts.Remove(k);
            else
                throw new ArgumentNullException();
            SaveProductList();
        }
        public void AddProduct(Product P)
        {
            Products.Add(P);
        }
        public void DeleteProduct(Product P)
        {
            if (Products.Contains(P))
                Products.Remove(P);
        }
        public IReadOnlyList<Product> GetList()
        {
            IReadOnlyList<Product> l = Products.ToList();
            return l;
        }
        private static void SaveProductList()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(@"data\pickled_store_products.dat", FileMode.Create, FileAccess.Write);
            bf.Serialize(fs, StoreProducts);
            fs.Close();
        }
        ~ProductList()
        {
            SaveProductList();
        }
    }
}
