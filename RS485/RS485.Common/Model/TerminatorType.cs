using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Serial.Model
{
    /// <summary>
    /// Type of terminator, sequence of characters ending each message
    /// </summary>
    public enum TerminatorType
    {
        [Description("Brak")]
        None,
        [Description("Carriage Return")]
        CR,
        [Description("Line Feed")]
        LF,
        [Description("CRLF")]
        CRLF,
        [Description("Własny")]
        Custom
    }
}