using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.Master.Serial.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Integration.Tests
{
    class StartMe
    {

        public const string portMaster = "COM0";
        public const string portSlave = "COM1";

        static void Main(string[] args)
        {
            c1_standardCase.run();
            MasterTest test = new MasterTest();
            try
            {
                test.execute(spawnConnectionSettings(portMaster), spawnConnectionSettings(portSlave));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                test._slavePort.CloseConnectionAsync();
                test._master.closePort();
                
            }
            Console.ReadLine();
        }

        public static ConnectionSettings spawnConnectionSettings(string portName)
        {
             ConnectionSettings settings = new ConnectionSettings();
            settings.BitRate = BitRate.BR_4800;
            settings.CharacterFormat = new CharacterFormat();
            settings.CharacterFormat.DataFieldSize = 8;
            settings.CharacterFormat.ParityControl = ParityControl.Even;
            settings.CharacterFormat.StopBitsNumber = StopBitsNumber.One;
            settings.FlowControl = FlowControl.Manual;
            settings.PortName = portName;
            settings.ReadTimeout = 500;
            settings.WriteTimeout = 500;
            settings.TerminalString = "00";
            return settings;
        }

        public static ModbusSettings spawnModbusSettings() {
             ModbusSettings modbus = new ModbusSettings();
             modbus.RetransmissionsCount = 5;
             modbus.TransactionDuration = 600;
             return modbus;
        }

        public static SlaveConfiguration spawnSlaveConfig(int address, String textBackToMaster)
        {
            SlaveConfiguration config = new SlaveConfiguration();
            config.SlaveAddress = address;
            config.TextSendBackToMaster = textBackToMaster;
            return config;
        }
    }
}
