using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial.Model
{
    
    class Frame
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
            this.deviceAddress = deviceAddress;
            this.message = message;
            this.lrc = lrc;
        }

        public string getStringToSend()
        {
            return deviceAddress + DATA_SEPARATOR + message + DATA_SEPARATOR + lrc;
        }
    }
}
