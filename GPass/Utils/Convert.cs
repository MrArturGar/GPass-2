using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPass.Utils
{
    class Convert
    {
        public static string BytesToString(byte[] _byte)
        {
            return Encoding.Default.GetString(_byte);
        }

        public static byte[] StringToBytes(string _string)
        {
            return Encoding.Default.GetBytes(_string);
        }
    }
}
