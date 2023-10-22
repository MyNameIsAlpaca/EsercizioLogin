using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.ConstrainedExecution;
using UtilityLib;

namespace EsercizioLogin.User
{
    internal class UserManager
    {

        PasswordGen passwordGen = new PasswordGen();
        bool close = false;
        Utility utility = new Utility();
        Utility.ErrorLog errorLog = new Utility.ErrorLog();

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
                                    utility.errorStyle("Password non corretta, riprova");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        utility.errorStyle("Utente non trovato, riprova");
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
            string registerLog = Path.Combine(relPath, "RegisterLog.txt");
            string errorLogPath = Path.Combine(relPath, "ErrorLog.txt");

            Console.Clear();
            while (!close)
            {
                Console.WriteLine("");
                Console.WriteLine("== Registrazione ==");
                Console.WriteLine("");

                Console.WriteLine("Inserisci un nome utente oppure inserisci f per tornare indietro");
                string userNameChoose = Console.ReadLine();

                if (userNameChoose.ToLower() == "f")
                {
                    Console.Clear();
                    return;
                }

                var userTryRegister = listUsers.SingleOrDefault(r => r.UserName.ToLower() == userNameChoose.ToLower());

                if (userTryRegister != null)
                {
                    Console.Clear();
                    utility.errorStyle("Questo nome utente esiste già, riprova");
                }
                else if (userNameChoose.Length < 3)
                {
                    Console.Clear();
                    utility.errorStyle("Il nome deve essere lungo almeno 3 caratteri");
                }
                else if (userNameChoose.Length > 20)
                {
                    Console.Clear();
                    utility.errorStyle("Il nome non può essere lungo più di 20 caratteri");
                }
                else
                {
                    Console.Clear();
                    while (!close)
                    {
                    Console.WriteLine("Scegli una password");
                    string userPasswordChoose = Console.ReadLine();

                    if(userPasswordChoose.Length < 5 )
                    {
                        Console.Clear();
                        utility.errorStyle("Scegli una password più efficace"); 
                    } 
                    else
                        {
                        string encPasswordChoose = passwordGen.PasswordEnc(userPasswordChoose);
                        int userId;

                        if (listUsers.Count > 0)
                        {
                            userId = listUsers[listUsers.Count - 1].id + 1;
                        }
                        else
                        {
                            userId = 1;
                        }

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

                            Utility.DataLog dataLog = new Utility.DataLog(this.GetType().Name, MethodBase.GetCurrentMethod().Name, DateTime.Now);
                            utility.WriteDataEvent(dataLog, registerLog, userNameChoose);
                            Console.Clear();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Registrazione completata");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception ex)
                        {
                            Console.Clear();

                            errorLog = new Utility.ErrorLog
                            (
                                errorLog.ClassName = this.GetType().Name, errorLog.MethodName = MethodBase.GetCurrentMethod().Name, errorLog.DateTimeLog = DateTime.Now,
                                errorLog.ErrorCode = ex.HResult, errorLog.ErrorMessage = ex.Message, errorLog.InnerExcept = ex.InnerException != null ? ex.InnerException.ToString() : ""
                            );
                            errorLog.WriteErrorEvent(errorLog, errorLogPath);
                            Console.WriteLine($"è stato riscontrato il seguente errore: {ex.Message}");
                        }
                        close = true;

                        }
                    }
                }
            }
        }
    }
}

