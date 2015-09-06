using RS485.Serial.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial
{
    public delegate void FirstCommandCompleted(CommandResult result);

    public delegate void SecondCommandCompleted(CommandResult result, String resultMessage);

    public delegate void ErrorOccured(String errorMessage);


    interface ModbusMaster
    {
        event FirstCommandCompleted firstCommandCompletedHandlers;
        event SecondCommandCompleted secondCommandCompletedHandlers;
        event ErrorOccured errorOccuredHandlers;

        //public EventHa
        void setConnectionSettings(ConnectionSettings settings);

        void setModbusSettings(ModbusSettings settings);

        void sendFirstCommand(String slaveAddress, String message);

        void sendSecondCommand(String slaveAddress);

    }

    public enum CommandResult
    {
        SUCCESS, FAIL
    }

}
