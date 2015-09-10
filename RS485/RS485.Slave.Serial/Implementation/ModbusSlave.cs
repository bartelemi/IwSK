using RS485.Common.Interfaces;
using System;
using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;
using RS485.Common.Serial;
using System.Diagnostics;

namespace RS485.Slave.Serial.Implementation
{
    public class ModbusSlave : IModbusSlave
    {
        public event ReceivedMessageFromMasterInFirstCommand FirstCommandReceived;
        public event SentTextToMasterInSecondCommand SecondCommandResponseSent;
        public event LogMessageOccuredEventHandler LogMessageOccured;

        private SlaveConfiguration slaveConfig;

        private ConnectionSettings connectionSettings;

        private SerialPortHandler serialPort = new SerialPortHandler();

        public ModbusSlave(ConnectionSettings settings, SlaveConfiguration config)
        {
            this.connectionSettings = settings;
            this.slaveConfig = config;
            serialPort.OnDataReceived += serialDataReceived;
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
             this.connectionSettings = settings;
        }

        public void SetSlaveConfiguration(SlaveConfiguration config)
        {
            this.slaveConfig = config;
        }

        public void StartListening()
        {
            try
            {
                serialPort.OpenConnectionAsync(connectionSettings);
                Debug.WriteLine("Port opened");
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return;
            }
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }



        public void serialDataReceived(string data, MessageType type)
        {
            Debug.WriteLine("Slave received: " + data);
        }
    }
}
