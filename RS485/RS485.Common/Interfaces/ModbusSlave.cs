using RS485.Common.Exceptions;
using RS485.Serial.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
