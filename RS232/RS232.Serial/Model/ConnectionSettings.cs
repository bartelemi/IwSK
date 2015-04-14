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
        public int ReadTimeout { get; set; }
        public int WriteTimeout { get; set; }
        public Encoding Encoding { get; set; }
        public string TerminalString { get; set; }
        public FlowControl FlowControl { get; set; }
        public CharacterFormat CharacterFormat { get; set; }
    }
}