using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.Serial.Model
{
    /// <summary>
    /// Number of stop bits after every character
    /// </summary>
    public enum StopBitsNumber
    {
        [Description("Brak")]
        Zero = 0,
        [Description("Jeden")]
        One = 1,
        [Description("Dwa")]
        Two = 2
    }
}