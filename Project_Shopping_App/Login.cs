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
        public static bool CheckUser(User usr)
        {
            if (File.Exists("data\\users\\" + usr.Name + ".dat"))
                return true;
            return false;
        }
        public static (Admin a, Customer c) LoadUser(User usr)
        {
            if (CheckUser(usr))
            {
                FileStream fs = new FileStream("data\\users\\" + usr.Name + ".dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                Admin tempa = bf.Deserialize(fs) as Admin;
                fs.Seek(0, SeekOrigin.Begin);
                Customer tempb = bf.Deserialize(fs) as Customer;
                fs.Close();
                if (tempa != null)
                {
                    if (tempa.Password == usr.Password)
                        return (tempa, null);
                }
                if (tempb != null)
                {
                    if (tempb.Password == usr.Password)
                        return (null, tempb);
                }
            }
            return (null, null);
        }
    }
}
