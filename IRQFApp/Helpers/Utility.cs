using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.Helpers
{
    class Utility
    {
        public static string ByteToEigthBits(byte item, bool reverse)
        {
            char[] arr = Convert.ToString(item, 2).ToCharArray();

            Array.Reverse(arr);
            string x = new string(arr);

            for (int i = x.Length; i < 8; i++)
            {
                x += "0";
            }


            return x;
        }

    }
}
