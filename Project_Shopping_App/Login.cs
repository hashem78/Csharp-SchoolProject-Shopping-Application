using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace shoppingApp
{
    sealed class Login
    {
        public static bool CheckUserExists(string username)
        {
            if (File.Exists(@"data\users\" + username + ".dat"))
                return true;
            return false;
        }
        public static User LoadUser(string username, string password)
        {
            if (CheckUserExists(username))
            {
                FileStream fs = new FileStream(@"data\users\" + username + ".dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                User U = bf.Deserialize(fs) as User;
                fs.Close();
                if (U.Password == password)
                    return U;
            }
            return null;
        }
        public static Customer GetCustomer(string username)
        {
            if (CheckUserExists(username))
            {
                FileStream fs = new FileStream(@"data\users\" + username + ".dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Customer C = bf.Deserialize(fs) as Customer;
                fs.Close();
                if (C != null)
                    return C;
            }
            return null;
        }
    }

}
