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
        PasswordGen passwordGen = new PasswordGen();
        bool close = false;

        public void loginUser(List<User> listUsers)
        {
            Console.Clear();
            while (!close)
            {
                Console.WriteLine("");
                Console.WriteLine("== Login ==");
                Console.WriteLine("");
                Console.WriteLine("Inserisci il tuo nome utente oppure premi f per tornare indietro");
                string userName = Console.ReadLine();

                if (userName.ToLower() == "f")
                {
                    Console.Clear();
                    close = true;
                }
                else
                {
                    var userTryLogin = listUsers.SingleOrDefault(r => r.UserName.ToLower() == userName.ToLower());

                    if (userTryLogin != null)
                    {
                        Console.Clear();
                        while (!close)
                        {
                            Console.WriteLine("Inserisci la password oppure premi f per tornare indietro");
                            string password = Console.ReadLine();

                            if (password.ToLower() == "f")
                            {
                                Console.Clear();
                                close = true;
                            }
                            else
                            {
                                string EncPassword = passwordGen.PasswordEnc(password);

                                if (EncPassword == userTryLogin.Password)
                                {
                                    Console.WriteLine("Sei loggato");
                                    Console.ReadLine();
                                    Console.Clear();
                                    close = true;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Password non corretta, riprova");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Utente non trovato, riprova");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
            close = false;
        }

        public void createUser(List<User> listUsers)
        {
            bool close = false;

            string relPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(relPath, "DataUser", "userList.xml");

            Console.Clear();
            while (!close)
            {

                Console.WriteLine("");
                Console.WriteLine("== Registrazione ==");
                Console.WriteLine("");


                Console.WriteLine("Inserisci un nome utente oppure inserisci f per tornare indietro");

                string userNameChoose = Console.ReadLine();

                if(userNameChoose.ToLower() == "f")
                {
                    Console.Clear();
                    return;
                }

                var userTryRegister = listUsers.SingleOrDefault(r => r.UserName.ToLower() == userNameChoose.ToLower());

                if(userTryRegister != null)
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("Questo nome utente esiste già, riprova");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Scegli una password");
                    string userPasswordChoose = Console.ReadLine();

                    string encPasswordChoose = passwordGen.PasswordEnc(userPasswordChoose);
                    id++;

                    int userId = id;
                    User user = new User(userId, userNameChoose, encPasswordChoose);

                    try
                    {
                    listUsers.Add(user);

                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

                    if (File.Exists(filePath))
                    {

                        using (StreamWriter sw = new($"{relPath}\\DataUser\\userList.xml"))
                        {
                            serializer.Serialize(sw, listUsers);
                        }
                    }
                    else
                    {

                        using (StreamWriter sw = new($"{relPath}\\DataUser\\userList.xml"))
                        {
                            serializer.Serialize(sw, listUsers);
                        }
                    }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Registrazione completata");
                    Console.ForegroundColor = ConsoleColor.White;

                    close = true;
                }
            }


        }
    }
}
