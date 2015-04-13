using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RS232.Serial.Model;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Represents message parameters
    /// </summary>
    public class MessageProperties
    {
        public bool AppendDateTime { get; set; }

        public string TerminalString { get; set; }
    }
}