using System;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;

namespace RS485.Common.Interfaces
{
    public delegate void ReceivedMessageFromMasterInFirstCommand(string messageReceived);
    public delegate void SentTextToMasterInSecondCommand();

    public interface IModbusSlave : IDisposable
    {
        event ReceivedMessageFromMasterInFirstCommand FirstCommandReceived;
        event SentTextToMasterInSecondCommand SecondCommandResponseSent;
        event LogMessageOccuredEventHandler LogMessageOccured;

        void SetConnectionSettings(ConnectionSettings settings);

        void SetSlaveConfiguration(SlaveConfiguration config);

        void StartListening();
    }
}
