using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.UI.Model
{
    /// <summary>
    /// Type of transmission control
    /// </summary>
    public enum TransmissionControl
    {
        [Description("Brak")]
        None,
        [Description("Bit parzystości")]
        Even,
        [Description("Bit nieparzystości")]
        Odd
    }
}
