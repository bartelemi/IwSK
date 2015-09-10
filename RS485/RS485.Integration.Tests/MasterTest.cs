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
using System.Threading.Tasks;

namespace RS485.Integration.Tests
{

    class MasterTest
    {
        public SerialPortHandler _slavePort = new SerialPortHandler();

        public IModbusMaster _master;

        private ConnectionSettings settingsMaster, settingsSlave;
        public void execute(ConnectionSettings settingsMaster, ConnectionSettings settingsSlave)
        {
            this.settingsMaster = settingsMaster;
            this.settingsSlave = settingsSlave;
            prepareSlavePort();
            prepareMaster();
            testStandardCaseFirstCommand();

            
        }

        private void prepareMaster()
        {
            _master = new ModbusMaster( settingsMaster, StartMe.spawnModbusSettings());
            _master.FirstCommandCompletedHandlers += FirstCommandCompleted;
        }

        private void testStandardCaseFirstCommand()
        {
            _slavePort.OnDataReceived += testStandardCaseFirstCommand_slaveHandler;
            _master.SendFirstCommand("10", "test message");
        }

        private void testStandardCaseFirstCommand_slaveHandler(string data, MessageType type)
        {
            Frame received = FrameBuilder.mapFromString(data);
            Debug.Assert(received.DeviceAddress.Equals("10"));
            Debug.Assert(received.Message.Equals("text message"));

            _slavePort.SendMessageAsync("ACK");
        }

        public  void FirstCommandCompleted(CommandResult result){
            if(result == CommandResult.FAIL)
                Debug.Assert(false);
        }

        private async void prepareSlavePort()
        {
            try
            {
                await _slavePort.OpenConnectionAsync(settingsSlave);
                Debug.WriteLine("Port opened");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }
    }
}
