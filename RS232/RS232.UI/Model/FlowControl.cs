using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.UI.Model
{
    /// <summary>
    /// Type of message flow control
    /// </summary>
    public enum FlowControl
    {
        [Description("Brak")]
        None,
        [Description("Sprzętowa")]
        Hardware,
        [Description("Programowa")]
        Software,
        [Description("Ręczna")]
        Manual
    }
}
