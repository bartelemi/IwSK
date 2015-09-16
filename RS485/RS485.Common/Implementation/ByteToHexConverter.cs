using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Implementation
{
    class ToHexConverter
    {

        public static string GetHexForm(String input)
        {
            byte[] ba = Encoding.Default.GetBytes(input);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }

        public static string GetHexForm(byte raw)
        {
            var array = new byte[1];
            array[0] = raw;
            var hexString = BitConverter.ToString(array);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
    }
}
