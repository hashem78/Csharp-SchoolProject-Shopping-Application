using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Everything in this file is supposed to be written by Hashem

namespace shoppingApp
{
    [Serializable]
    class Order
    {
        public Customer Customer;
        public DateTime TimeOfPurchase;
        public string OrderId;
        public double Cost;
        public Order(Customer customer,double cost)
        {
            TimeOfPurchase = DateTime.Now;
            this.Customer = customer;
            this.Cost = cost;
            GenerateOrderId();
            
            SaveOrder();
        }
        public void Print()
        {
            Console.WriteLine("Order date {0}, with a total of {1}", TimeOfPurchase.ToShortDateString(), Cost);
            Customer.ViewList();
            Console.WriteLine();
            foreach (Product p in Customer.Basket.List.Values)
            {
                Console.WriteLine("Ordered {0} of ID {1} on {2}", p.ProductQuantity, p.ProductId, p.TimeOfPurchase.ToShortDateString());
            }
            Console.WriteLine("///////////////////");
        }
        private void GenerateOrderId()
        {
            OrderId = ("#"+ Customer.Name + TimeOfPurchase.ToShortDateString()).Replace('/','0');
        }
        private void SaveOrder()
        {
            FileStream fs = new FileStream(@"data\orders\" + OrderId + ".dat",FileMode.Create,FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs,this);
            fs.Close();
        }
        public static Order GetOrder(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Order o = (Order)bf.Deserialize(fs);
                fs.Close();
                return o;
            }
            return null;
        }
    }
}
