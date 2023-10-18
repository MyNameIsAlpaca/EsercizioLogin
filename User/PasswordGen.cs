using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioLogin.User
{
    internal class PasswordGen
    {
        public string PasswordEnc(string passValue) 
        {
            SHA256 sha256 = SHA256.Create();

            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passValue));

            string EncryptedResult = string.Empty;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return EncryptedResult = sb.ToString();
        }
    }
}
