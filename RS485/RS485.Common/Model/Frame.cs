using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Model
{
    
    public class Frame
    {
        private string deviceAddress;
        private string message;
        private string lrc;

        public const string DATA_SEPARATOR = ";";

        public string DeviceAddress
        {
            get { return this.deviceAddress; }
        }

        public string Message
        {
            get { return this.message; }
        }

        public String Lrc
        {
            get { return this.lrc; }
        }

        public Frame(string deviceAddress, string message, string lrc)
        {
            this.deviceAddress = normalizeAddress(deviceAddress);
            this.message = message;
            this.lrc = lrc;

        }

        public static string normalizeAddress(string deviceAddress)
        {
            if (deviceAddress.Length == 1)
            {
                return "00" + deviceAddress;
            } else if (deviceAddress.Length == 2)
            {
                return "0" + deviceAddress;
            }
            else
            {
                return deviceAddress;
            }
            
        }

        public string getStringToSend()
        {

            return deviceAddress + message + lrc;
        }

        public string getHexForm()
        {
            byte[] ba = Encoding.Default.GetBytes(deviceAddress + DATA_SEPARATOR + message + DATA_SEPARATOR + lrc);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");
            return hexString;
        }
    }
}
