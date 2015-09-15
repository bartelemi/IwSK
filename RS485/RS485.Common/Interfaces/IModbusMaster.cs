using System;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;

namespace RS485.Common.Interfaces
{
    public delegate void FirstCommandCompletedEventHandler(CommandResult result);
    public delegate void SecondCommandCompletedEventHandler(CommandResult result, string resultMessage);

    public interface IModbusMaster : IDisposable
    {
        event FirstCommandCompletedEventHandler FirstCommandCompleted;
        event SecondCommandCompletedEventHandler SecondCommandCompleted;
        event LogMessageOccuredEventHandler LogMessageOccured;

        void SetConnectionSettings(ConnectionSettings settings);

        void SetModbusSettings(ModbusSettings settings);

        void SendFirstCommand(string slaveAddress, string message);

        void SendSecondCommand(string slaveAddress);
    }
}