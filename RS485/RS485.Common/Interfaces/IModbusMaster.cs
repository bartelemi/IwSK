using System;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;

namespace RS485.Common.Interfaces
{
    public delegate void FirstCommandCompleted(CommandResult result);

    public delegate void SecondCommandCompleted(CommandResult result, String resultMessage);

    public interface IModbusMaster
    {
        event FirstCommandCompleted FirstCommandCompletedHandlers;
        event SecondCommandCompleted SecondCommandCompletedHandlers;
        event LogMessageOccuredEventHandler LogMessageOccured;

        void SetConnectionSettings(ConnectionSettings settings);

        void SetModbusSettings(ModbusSettings settings);

        void SendFirstCommand(String slaveAddress, String message);

        void SendSecondCommand(String slaveAddress);

        void closePort();
    }
}