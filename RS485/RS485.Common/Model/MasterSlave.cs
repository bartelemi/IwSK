using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Model
{
        /// <summary>
    /// Master or Slave Mode
    /// </summary>
    public enum MasterSlave
    {
        [Description("Master")]
        Master = 1,
        [Description("Slave")]
        Slave = 2,
    }
    
}
