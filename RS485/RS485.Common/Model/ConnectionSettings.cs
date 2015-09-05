﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS485.Serial.Model
{
    public class ConnectionSettings
    {
        public BitRate BitRate { get; set; }        
        public int ReadTimeout { get; set; }
        public int WriteTimeout { get; set; }
        public Encoding Encoding { get; set; }
        public string TerminalString { get; set; }
        public FlowControl FlowControl { get; set; }
        public CharacterFormat CharacterFormat { get; set; }
    }
}