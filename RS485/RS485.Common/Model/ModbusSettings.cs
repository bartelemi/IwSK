using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Serial.Model
{
    public class ModbusSettings
    {
        public int RetransmissionsCount { get; set; }
        public int TransactionDuration { get; set; }

    }
}
