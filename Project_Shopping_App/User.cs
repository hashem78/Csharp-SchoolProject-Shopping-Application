using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace shoppingApp
{
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
        public virtual void AddProduct(Product product)
        {

        }
        public virtual void ViewProducts(double threshold)
        {

        }
    }
    class Admin : User
    {
        public Admin(string Name, string Password) : base(Name, Password)
        {

        }
        public override void AddProduct(Product product)
        {
            ProductList.AddStoreProduct(product);
        }
        public override void ViewProducts(double threshold)
        {
            string space = "            ";
            Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
            foreach (Product product in ProductList.GetStoreList())
            {
                if (product.ProductQuantity >= threshold)
                    product.Print();
            }
        }

    }
    class Customer : User
    {
        ProductList Basket = new ProductList();
        public Customer(string Name, string Password): base(Name, Password)
        { 

        }
        public override void AddProduct(Product product)
        {
            Basket.AddProduct(product);
        }
        public override void ViewProducts(double threshold = 0)
        {
            if (threshold == 0)
            {
                string space = "            ";
                Console.WriteLine("ID" + space + "Name" + space + "Category" + space + "Price" + space + "Quantity");
                foreach (Product product in Basket.GetList())
                {
                    product.Print();
                }
            }
        }
        ~Customer()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(this.Name + ".dat", FileMode.Create, FileAccess.Write);
            bf.Serialize(fs,Basket);
            bf.Serialize(fs, Name);
            bf.Serialize(fs, Password);
            fs.Close();
        }
    }
}
