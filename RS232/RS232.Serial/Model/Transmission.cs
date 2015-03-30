using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Type of transmission
    /// </summary>
    public enum Transmission
    {
        [Description("Binarna")]
        Binary,
        [Description("Tekstowa")]
        Text
    }
}