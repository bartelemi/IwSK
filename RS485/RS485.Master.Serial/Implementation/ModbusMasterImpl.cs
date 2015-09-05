using RS485.Master.Serial.Exceptions;
using RS485.Serial.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Master.Serial.Implementation
{
    class ModbusMasterImpl : ModbusMaster
    {

        public event FirstCommandCompleted firstCommandCompletedHandlers;

        public event SecondCommandCompleted secondCommandCompletedHandlers;

        public event ErrorOccured errorOccuredHandlers;

        private ModbusSettings modbusSettings;

        private ConnectionSettings connectionSettings;

        public void setConnectionSettings(ConnectionSettings settings)
        {
            this.connectionSettings = settings;
        }

        public void setModbusSettings(ModbusSettings settings)
        {
            this.modbusSettings = settings;
        }

        public void sendFirstCommand(string slaveAddress, string message)
        {
            throw new NotImplementedException();
        }

        public void sendSecondCommand(string slaveAddress)
        {
            if ("0".Equals(slaveAddress))
            {
                throw new InvalidArgumentException("Operacja nr. 2 może być realizowana tylko dla transmisji adresowanej!");
            }

        }
    }
}
