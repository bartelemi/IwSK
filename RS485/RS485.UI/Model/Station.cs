using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.UI.Model
{
    /// <summary>
    /// Type of station
    /// </summary>
    public enum Station
    {
        [Description("Master")]
        Master,
        [Description("Slave")]
        Slave
    }
}