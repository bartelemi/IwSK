using RS485.Common.Interfaces;
using RS485.Master.Serial.Implementation;
using RS485.Slave.Serial.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Integration.Tests
{
    class c1_standardCase
    {

        public static void run()
        {
            IModbusSlave slave = new ModbusSlave(StartMe.spawnConnectionSettings(StartMe.portSlave), StartMe.spawnSlaveConfig(10, "back"));
            slave.StartListening();

            IModbusMaster master = new ModbusMaster(StartMe.spawnConnectionSettings(StartMe.portMaster), StartMe.spawnModbusSettings());
            master.SendFirstCommand("20", "works motherfucker");
        }
    }
}
