using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.UI.Model
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
