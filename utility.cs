using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioLogin
{
    internal class utility
    {
        public bool testInt(string value) 
        {
            if (int.TryParse(value, out int val))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
    }
}
