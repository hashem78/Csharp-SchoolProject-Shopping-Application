using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shoppingApp
{
    class Program
    {
        public static void AddProduct(User usr,Product P)
        {
            usr.AddProduct(P);
        }
        static void Main(string[] args)
        {
            Admin admn = new Admin("admin", "00");
            Customer usr = new Customer("Ahmad", "11");
        }
    }
}
