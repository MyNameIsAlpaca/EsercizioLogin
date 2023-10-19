using EsercizioLogin.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UtilityLib;


namespace EsercizioLogin
{
    internal class Menu
    {
        public void openMenu()
        {
            
            string relPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string filePath = Path.Combine(relPath, "DataUser", "userList.xml");

            userList userList = new userList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<User.User>));

            Utility utility = new Utility();

            if (File.Exists(filePath))
            {
                userList.listUsers = new List<User.User>();

                using(StreamReader xmlFile = new StreamReader(filePath))
                {
                    userList.listUsers = (List<User.User>)serializer.Deserialize(xmlFile);
                }
            }


            UserManager userManager = new UserManager();

            bool close = false;

            while (!close)
            {
                Console.WriteLine("");
                Console.WriteLine("== Benvenuto ==");
                Console.WriteLine("");
                Console.WriteLine("Sei già registrato?\n1) Si\n2) No\n3) Esci");
                string userChoose = Console.ReadLine();
                if (utility.testInt(userChoose))
                {
                    switch(userChoose)
                    {
                        case "1":

                            userManager.loginUser(userList.listUsers);

                            break;

                        case "2":

                            userManager.createUser(userList.listUsers);

                            break;

                        default:
                            close = true;
                            break;
                        
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Scegli un'opzione valida");
                }
            }
        }
    }
}
