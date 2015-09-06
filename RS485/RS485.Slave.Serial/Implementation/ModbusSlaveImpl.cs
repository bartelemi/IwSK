using RS485.Common.Exceptions;
using RS485.Common.Interfaces;
using System;
using RS485.Common.Model;

namespace RS485.Slave.Serial.Implementation
{
    public class ModbusSlaveImpl : ModbusSlave
    {

        public event ErrorOccured errorOccuredHandlers;

        public event ReceivedMessageFromMasterInFirstCommand firstCommandReceived;

        public event SentTextToMasterInSecondCommand secondCommandResponseSent;

        public void setConnectionSettings(ConnectionSettings settings)
        {
            throw new NotImplementedException();
        }

        public void setSlaveConfiguration(SlaveConfiguration config)
        {
            throw new NotImplementedException();
        }

        public void startListening()
        {
            throw new NotImplementedException();
        }
    }
}
