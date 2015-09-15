using RS485.Common.Interfaces;
using RS485.Master.Serial.Implementation;
using RS485.Slave.Serial.Implementation;

namespace RS485.Integration.Tests
{
    class C1StandardCase
    {
        public static void Run()
        {
            IModbusSlave slave = new ModbusSlave(StartMe.SpawnConnectionSettings(StartMe.PortSlave), StartMe.SpawnSlaveConfig(10, "back"));
            slave.StartListening();

            IModbusMaster master = new ModbusMaster(StartMe.SpawnConnectionSettings(StartMe.PortMaster), StartMe.SpawnModbusSettings());
            master.SendFirstCommand("20", "works motherfucker");
        }
    }
}
