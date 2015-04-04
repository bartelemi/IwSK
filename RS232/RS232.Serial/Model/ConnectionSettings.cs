using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RS232.Serial.Model;

namespace RS232.Serial
{
    public class ConnectionSettings
    {
        public string PortName { get; set; }
        public BitRate BitRate { get; set; }
        public int DataBits { get; set; }
        public ParityControl ParityControl { get; set; }
        public CharacterFormat CharacterFormat { get; set; }
    }
}