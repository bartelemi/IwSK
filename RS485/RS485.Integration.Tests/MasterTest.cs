using RS485.Common.Implementation;
using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.Common.Serial;
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

    class MasterTest
    {
        public SerialPortHandler _slavePort = new SerialPortHandler();

        bool slavePortOpened = false;

        public IModbusMaster _master;

        private ConnectionSettings settingsMaster, settingsSlave;
        public void prepareExecute(ConnectionSettings settingsMaster, ConnectionSettings settingsSlave)
        {
            this.settingsMaster = settingsMaster;
            this.settingsSlave = settingsSlave;
            prepareSlavePort();
            prepareMaster();
        }

        private void prepareMaster()
        {
            _master = new ModbusMaster( settingsMaster, StartMe.spawnModbusSettings());
            _master.FirstCommandCompletedHandlers += FirstCommandCompleted;
        }

        public void testStandardCaseFirstCommand()
        {
            _slavePort.OnDataReceived += testStandardCaseFirstCommand_slaveHandler;
            _master.SendFirstCommand("10", "test standard case");
        }

        private async void testStandardCaseFirstCommand_slaveHandler(string data, MessageType type)
        {
            Frame received = FrameBuilder.mapFromString(data);
            Debug.Assert(received.DeviceAddress.Equals("10"));
            Debug.Assert(received.Message.Equals("test standard case"));
            Debug.WriteLine("Slave Received: " + data);
            await  _slavePort.SendMessageAsync(CommandResult.SUCCESS.ToString());
            Console.WriteLine("1. standard case sucess");
            Thread.Sleep(300);
        }

        private int retransmissionsCount = 0;

        public void testRetransmission()
        {
            _slavePort.OnDataReceived += testRetransmission_slaveHandler;
            _slavePort.OnDataReceived -= testStandardCaseFirstCommand_slaveHandler;
            _master.SendFirstCommand("10", "test retrans");
        }

        private void testRetransmission_slaveHandler(string data, MessageType type)
        {
            Frame received = FrameBuilder.mapFromString(data);
            Debug.Assert(received.DeviceAddress.Equals("10"));
            Debug.Assert(received.Message.Equals("test retrans"));
            Debug.WriteLine("Slave Received: " + data);
            if (retransmissionsCount == 2)
            {
                _slavePort.SendMessageAsync(CommandResult.SUCCESS.ToString());
                Console.WriteLine("2. retransmission success");
                _slavePort.OnDataReceived -= testRetransmission_slaveHandler;
            }
            else
            {
                retransmissionsCount++;
            }
        }

        public  void FirstCommandCompleted(CommandResult result){
            if(result == CommandResult.FAIL)
                Debug.Assert(false);
        }

        private void prepareSlavePort()
        {
            try
            {
                _slavePort.OpenConnectionAsync(settingsSlave);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw;
            }
            slavePortOpened = true;
            Debug.WriteLine("Slave port opened");
        }
    }
}
