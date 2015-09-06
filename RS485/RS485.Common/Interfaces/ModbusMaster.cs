﻿using RS485.Common.Exceptions;
using RS485.Common.Model;
using System;

namespace RS485.Common.Interfaces
{
    public delegate void FirstCommandCompleted(CommandResult result);

    public delegate void SecondCommandCompleted(CommandResult result, String resultMessage);

    public interface ModbusMaster
    {
        event FirstCommandCompleted firstCommandCompletedHandlers;
        event SecondCommandCompleted secondCommandCompletedHandlers;
        event ErrorOccured errorOccuredHandlers;

        void setConnectionSettings(ConnectionSettings settings);

        void setModbusSettings(ModbusSettings settings);

        void sendFirstCommand(String slaveAddress, String message);

        void sendSecondCommand(String slaveAddress);

    }


}
