using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.Master.Serial.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RS485.Integration.Tests
{
    class StartMe
    {

        public const string portMaster = "COM4";
        public const string portSlave = "COM5";

        static void Main(string[] args)
        {
            MasterTest test = new MasterTest();
            try
            {
                test.prepareExecute(spawnConnectionSettings(portMaster), spawnConnectionSettings(portSlave));
                test.testStandardCaseFirstCommand();
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
            Thread.Sleep(200);
            MasterTest test2 = new MasterTest();
            try
            {
                test2.prepareExecute(spawnConnectionSettings(portMaster), spawnConnectionSettings(portSlave));
                test2.testRetransmission();
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
             settings.BitRate = BitRate.BR_9600;
            settings.CharacterFormat = new CharacterFormat();
            settings.CharacterFormat.DataFieldSize = 8;
            settings.CharacterFormat.ParityControl = ParityControl.Even;
            settings.CharacterFormat.StopBitsNumber = StopBitsNumber.One;
            settings.FlowControl = FlowControl.Manual;
            settings.PortName = portName;
            settings.ReadTimeout = 500;
            settings.WriteTimeout = 5000;
            settings.TerminalString = "###";
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
