using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace shoppingApp
{
    class Login
    {
        public static bool CheckUserExists(string usr)
        {
            if (File.Exists(@"data\users\" + usr + ".dat"))
                return true;
            return false;
        }
        public static (Admin a, Customer c) LoadUser(string username,string password)
        {
            if (CheckUserExists(username))
            {
                FileStream fs = new FileStream(@"data\users\" + username + ".dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Admin tempa = bf.Deserialize(fs) as Admin;
                fs.Seek(0, SeekOrigin.Begin);
                Customer tempc = bf.Deserialize(fs) as Customer;
                fs.Close();
                if (tempa != null)
                        if (tempa.Password == password)
                            return (tempa, null);
                if (tempc != null)
                        if (tempc.Password == password)
                            return (null, tempc);
            }
            return (null, null);
        }
        public static Customer LoadCustomer(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Admin tempa = bf.Deserialize(fs) as Admin;
                fs.Seek(0, SeekOrigin.Begin);
                Customer tempc = bf.Deserialize(fs) as Customer;
                fs.Close();
                if (tempc != null)
                    return tempc;
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
