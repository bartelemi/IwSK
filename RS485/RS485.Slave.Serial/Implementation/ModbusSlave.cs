using RS485.Common.Interfaces;
using System;
using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;

namespace RS485.Slave.Serial.Implementation
{
    public class ModbusSlave : IModbusSlave
    {
        public event ReceivedMessageFromMasterInFirstCommand FirstCommandReceived;
        public event SentTextToMasterInSecondCommand SecondCommandResponseSent;
        public event LogMessageOccuredEventHandler LogMessageOccured;

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        public void SetSlaveConfiguration(SlaveConfiguration config)
        {
            throw new NotImplementedException();
        }

        public void StartListening()
        {
            throw new NotImplementedException();
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }
    }
}
