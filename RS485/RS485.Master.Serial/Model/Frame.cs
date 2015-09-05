using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial.Model
{
    
    class Frame
    {
        private string slaveAddress;
        private string message;
        private string lrc;

        public string SlaveAddress
        {
            get { return this.slaveAddress; }
        }

        public string Message
        {
            get { return this.message; }
        }

        public String Lrc
        {
            get { return this.lrc; }
        }

        public Frame(string slaveAddress, string message, string lrc)
        {
            this.slaveAddress = slaveAddress;
            this.message = message;
            this.lrc = lrc;
        }
    }
}
