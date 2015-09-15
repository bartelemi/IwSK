using RS485.Common.Implementation;
using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.Common.Serial;
using RS485.Master.Serial.Implementation;
using System;
using System.Diagnostics;
using System.Threading;

namespace RS485.Integration.Tests
{

    class MasterTest
    {
        public readonly SerialPortHandler SlavePort = new SerialPortHandler();

        bool _slavePortOpened = false;

        public IModbusMaster Master;

        private ConnectionSettings _settingsMaster, _settingsSlave;
        public void PrepareExecute(ConnectionSettings settingsMaster, ConnectionSettings settingsSlave)
        {
            _settingsMaster = settingsMaster;
            _settingsSlave = settingsSlave;
            PrepareSlavePort();
            PrepareMaster();
        }

        private void PrepareMaster()
        {
            Master = new ModbusMaster(_settingsMaster, StartMe.SpawnModbusSettings());
            Master.FirstCommandCompleted += FirstCommandCompleted;
        }

        public void TestStandardCaseFirstCommand()
        {
            SlavePort.OnDataReceived += testStandardCaseFirstCommand_slaveHandler;
            Master.SendFirstCommand("10", "test standard case");
        }

        private async void testStandardCaseFirstCommand_slaveHandler(string data, MessageType type)
        {
            Frame received = FrameBuilder.mapFromString(data);
            Debug.Assert(received.DeviceAddress.Equals("10"));
            Debug.Assert(received.Message.Equals("test standard case"));
            Debug.WriteLine("Slave Received: " + data);
            await  SlavePort.SendMessageAsync(CommandResult.Success.ToString());
            Console.WriteLine("1. standard case sucess");
            Thread.Sleep(300);
        }

        private int _retransmissionsCount = 0;

        public void TestRetransmission()
        {
            SlavePort.OnDataReceived += testRetransmission_slaveHandler;
            SlavePort.OnDataReceived -= testStandardCaseFirstCommand_slaveHandler;
            Master.SendFirstCommand("10", "test retrans");
        }

        private void testRetransmission_slaveHandler(string data, MessageType type)
        {
            Frame received = FrameBuilder.mapFromString(data);
            Debug.Assert(received.DeviceAddress.Equals("10"));
            Debug.Assert(received.Message.Equals("test retrans"));
            Debug.WriteLine("Slave Received: " + data);
            if (_retransmissionsCount == 2)
            {
                SlavePort.SendMessageAsync(CommandResult.Success.ToString());
                Console.WriteLine("2. retransmission success");
                SlavePort.OnDataReceived -= testRetransmission_slaveHandler;
            }
            else
            {
                _retransmissionsCount++;
            }
        }

        public void FirstCommandCompleted(CommandResult result){
            if(result == CommandResult.Fail)
                Debug.Assert(false);
        }

        private void PrepareSlavePort()
        {
            try
            {
                SlavePort.OpenConnectionAsync(_settingsSlave);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw;
            }
            _slavePortOpened = true;
            Debug.WriteLine("Slave port opened");
        }
    }
}
