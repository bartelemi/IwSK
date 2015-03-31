using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Input type for messages
    /// </summary>
    public enum InputType
    {
        [Description("Binarna")]
        Binary,
        [Description("Tekstowa")]
        Text
    }
}