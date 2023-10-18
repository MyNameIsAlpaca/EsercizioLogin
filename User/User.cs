using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioLogin.User
{
    public class User
    {
        User() { }

        public int id;
        public string UserName { get; set;}
        public string Password { get; set;}
        public User(int id, string UserName, string Password)
        {
            this.id = id;
            this.UserName = UserName;
            this.Password = Password;
        }
    }
}
