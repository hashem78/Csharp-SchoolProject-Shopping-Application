using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Hashem ziad hashem alayan
// Jafar feras jafar aljuneidi

/*
 * @namespace shoppingApp: contains all classes of this program
 * @class shoppingApp.Program: contains the main sequence
 * @class shoppingApp.User: base class of both shoppingApp.Admin & shoppingApp.Customer
 * @class shoppingApp.Product: custom type to handle products
 * @class sealed shoppingApp.ProductList: static wrapper around a hastable, handels products to be bought by the customers
 * @class shoppingApp.Basket: wrapper around a hashtable, all shoppingApp.Customer define an instance from this type
 * @class shoppingApp.Login: static class that handels login
 * @class shoppingApp.Orders: contains orders issued after every call from shoppingApp.Customer.Pay()
 */

namespace shoppingApp
{
    class Program
    {

        static void Main(string[] args)
        {
            //Admin's username is "admin", password is "00"
            //rest of users are like worksheet
            Login.Menu();
        }
    }
}
