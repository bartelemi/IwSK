using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Common.Interfaces
{
    public class SlaveConfiguration
    {
        public int slaveAddress { get; set; }
        public string textSendBackToMaster { get; set;}
    }
}
