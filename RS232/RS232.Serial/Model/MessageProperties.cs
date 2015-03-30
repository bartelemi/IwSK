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
        public Terminator Terminator { get; set; }
        public string CustomTerminator { get; set; }
        public bool AppendDateTime { get; set; }
    }
}