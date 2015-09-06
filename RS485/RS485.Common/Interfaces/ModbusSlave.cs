using RS485.Common.Exceptions;
using System;
using RS485.Common.Model;

namespace RS485.Common.Interfaces
{

    public delegate void ReceivedMessageFromMasterInFirstCommand(String messageReceived);
    public delegate void SentTextToMasterInSecondCommand();
    public interface ModbusSlave
    {
        event ErrorOccured errorOccuredHandlers;
        event ReceivedMessageFromMasterInFirstCommand firstCommandReceived;
        event SentTextToMasterInSecondCommand secondCommandResponseSent;

        void setConnectionSettings(ConnectionSettings settings);

        void setSlaveConfiguration(SlaveConfiguration config);

        void startListening();
    }
}
