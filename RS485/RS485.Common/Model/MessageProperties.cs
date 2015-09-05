using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS485.Serial.Model
{
    /// <summary>
    /// Represents message parameters
    /// </summary>
    public class MessageProperties
    {
        public MessageType MessageType { get; set; }
        public bool AppendDateTime { get; set; }
        public string TerminalString { get; set; }
    }
}