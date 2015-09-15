using System;
using RS485.Common.Interfaces;
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

        private SlaveConfiguration _slaveConfig;
        private ConnectionSettings _connectionSettings;
        private readonly SerialPortHandler _serialPort = new SerialPortHandler();

        public ModbusSlave(ConnectionSettings settings, SlaveConfiguration config)
        {
            _connectionSettings = settings;
            _slaveConfig = config;
            _serialPort.OnDataReceived += SerialDataReceived;
        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
             _connectionSettings = settings;
        }

        public void SetSlaveConfiguration(SlaveConfiguration config)
        {
            _slaveConfig = config;
        }

        public void StartListening()
        {
            try
            {
                _serialPort.OpenConnectionAsync(_connectionSettings);
                Debug.WriteLine("Port opened");
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }



        public void SerialDataReceived(string data, MessageType type)
        {
            Debug.WriteLine("Slave received: " + data);
        }

        public void Dispose()
        {
            
        }
    }
}
