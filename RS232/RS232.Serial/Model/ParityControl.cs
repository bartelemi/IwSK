using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Type of transmission control
    /// </summary>
    public enum ParityControl
    {
        [Description("Brak")]
        None = 0,
        [Description("Bit nieparzystości")]
        Odd = 1,
        [Description("Bit parzystości")]
        Even = 2
    }
}