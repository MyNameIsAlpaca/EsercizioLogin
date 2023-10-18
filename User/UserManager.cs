using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.ConstrainedExecution;

namespace EsercizioLogin.User
{
    internal class UserManager
    {

        int id = 0;

        utility utility = new utility();

        PasswordGen passwordGen = new PasswordGen();

        public void loginUser(List<User> listUsers)
        {
            Console.WriteLine(listUsers.Count);
            Console.ReadLine();
        }

        public void createUser(List<User> listUsers)
        {
            string relPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string filePath = Path.Combine(relPath, "DataUser", "userList.xml");

            Console.WriteLine("|=================|");
            Console.WriteLine("|| Registrazione ||");
            Console.WriteLine("|=================|");

            Console.WriteLine("Inserisci un nome utente");

            string userNameChoose = Console.ReadLine();

            Console.WriteLine("Scegli una password");

            string userPasswordChoose = Console.ReadLine();

            string encPasswordChoose = passwordGen.PasswordEnc(userPasswordChoose);

            id++;

            int userId = id;

            User user = new User(userId, userNameChoose, encPasswordChoose);

            Console.WriteLine("Registrazione completata");

            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            if (File.Exists(filePath))
            {
                using (StreamReader xmlFile = new StreamReader(filePath))
                {
                    listUsers = (List<User>)serializer.Deserialize(xmlFile);
                }

                listUsers.Add(user);

                using (StreamWriter sw = new($"{relPath}\\DataUser\\userList.xml"))
                {
                    serializer.Serialize(sw, listUsers);
                }
            }
            else
            {
                listUsers.Add(user);

                using (StreamWriter sw = new($"{relPath}\\DataUser\\userList.xml"))
                {
                    serializer.Serialize(sw, listUsers);
                }

            }

        }
    }
}
