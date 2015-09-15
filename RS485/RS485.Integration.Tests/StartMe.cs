using RS485.Common.Interfaces;
using RS485.Common.Model;
using System;
using System.Diagnostics;
using System.Threading;

namespace RS485.Integration.Tests
{
    static class StartMe
    {

        public const string PortMaster = "COM4";
        public const string PortSlave = "COM5";

        static void Main(string[] args)
        {
            MasterTest test = new MasterTest();
            try
            {
                test.PrepareExecute(SpawnConnectionSettings(PortMaster), SpawnConnectionSettings(PortSlave));
                test.TestStandardCaseFirstCommand();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            Thread.Sleep(200);
            try
            {
                test.TestRetransmission();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                test.SlavePort.CloseConnectionAsync();
                test.Master.Dispose();

            }
            Console.ReadLine();
        }

        public static ConnectionSettings SpawnConnectionSettings(string portName)
        {
            ConnectionSettings settings = new ConnectionSettings
            {
                BitRate = BitRate.BR_9600,
                CharacterFormat = new CharacterFormat
                {
                    DataFieldSize = 8,
                    ParityControl = ParityControl.Even,
                    StopBitsNumber = StopBitsNumber.One
                },
                FlowControl = FlowControl.Manual,
                PortName = portName,
                ReadTimeout = 500,
                WriteTimeout = 5000,
                TerminalString = "###"
            };
            return settings;
        }

        public static ModbusSettings SpawnModbusSettings() 
        {
            ModbusSettings modbus = new ModbusSettings
            {
                RetransmissionsCount = 5,
                TransactionDuration = 600
            };
            return modbus;
        }

        public static SlaveConfiguration SpawnSlaveConfig(int address, String textBackToMaster)
        {
            SlaveConfiguration config = new SlaveConfiguration
            {
                SlaveAddress = address,
                TextSendBackToMaster = textBackToMaster
            };
            return config;
        }
    }
}
