using RS485.Common.Serial;
using RS485.Common.Model;
using RS485.Common.Implementation;
using RS485.Master.Serial.Exceptions;
using RS485.Master.Serial.Model;
using RS485.Serial.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using RS485.Common.Exceptions;
using RS485.Common.Interfaces;

namespace RS485.Master.Serial.Implementation
{
    public class ModbusMasterImpl : ModbusMaster
    {

        public event FirstCommandCompleted firstCommandCompletedHandlers;

        public event SecondCommandCompleted secondCommandCompletedHandlers;

        public event ErrorOccured errorOccuredHandlers;

        private ModbusSettings modbusSettings;

        private ConnectionSettings connectionSettings;

        private MessageProperties messageProperties;

        private SerialPortHandler serialPort = new SerialPortHandler();

        private Transaction currentTransaction;

        private string lastReceivedData = "";

        public void setConnectionSettings(ConnectionSettings settings)
        {
            this.connectionSettings = settings;
        }

        public void setModbusSettings(ModbusSettings settings)
        {
            this.modbusSettings = settings;
        }

        public ModbusMasterImpl(ConnectionSettings connectionSettings, ModbusSettings modbusSettings)
        {
            this.connectionSettings = connectionSettings;
            this.modbusSettings = modbusSettings;
            this.serialPort.OnDataReceived += receivedDataHandler;

        }

        public async void sendFirstCommand(string slaveAddress, string message)
        {
            try
            {
                await serialPort.OpenConnectionAsync(connectionSettings);
                Debug.WriteLine("Port opened");
            }
            catch (Exception e)
            {
                errorOccuredHandlers(e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return;
            }
            int retransmissionsLeft = modbusSettings.RetransmissionsCount;
            Frame frameToSend = FrameBuilder.buildFrame(slaveAddress, message);
            Debug.WriteLine("Frame builded: " + frameToSend);
            do
            {
                currentTransaction = new Transaction(modbusSettings.TransactionDuration);
                try
                {
                    sendFrame(frameToSend);
                }
                catch (Exception e)
                {
                    errorOccuredHandlers(e.Message);
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    return;
                }
                while (currentTransaction.isTransactionStillActive())
                {
                    Debug.WriteLine("Transaction still active, wait for receive");
                    if (isFrameSendSuccess())
                    {
                        Debug.WriteLine("Received some data: " + lastReceivedData);
                        if (lastReceivedData.Equals(CommandResult.SUCCESS))
                        {
                            Debug.WriteLine("Tranmission success!");
                            sendFirstCommandCompletedEventSuccess();
                            return;
                        }
                        else
                        {
                            Debug.WriteLine("Tranmission fail, wrong ACK!");
                            sendFirstCommandCompletedEventFail();
                            return;
                        }
                    }
                    Thread.Sleep(5);
                }
                Debug.WriteLine("transaction timeout, decrease retransmission");
                retransmissionsLeft--;

            } while (retransmissionsLeft > 0);
            Debug.WriteLine("Communication fail, no ACK received");
            sendFirstCommandCompletedEventFail();
        }

        private void sendFirstCommandCompletedEventFail()
        {
            lastReceivedData = "";
            firstCommandCompletedHandlers(CommandResult.FAIL);
        }

        private void sendFirstCommandCompletedEventSuccess()
        {
            lastReceivedData = "";
            firstCommandCompletedHandlers(CommandResult.SUCCESS);
        }


        private void sendFrame(Frame frameToSend)
        {
            serialPort.SendMessageAsync(frameToSend.getStringToSend());
        }

        private bool isFrameSendSuccess()
        {
            return !lastReceivedData.Equals("ERROR");
        }

        public void receivedDataHandler(string data, MessageType type)
        {
            this.lastReceivedData = data;
        }

        public async void sendSecondCommand(string slaveAddress)
        {
            if ("0".Equals(slaveAddress))
            {
                throw new InvalidArgumentException("Operacja nr. 2 może być realizowana tylko dla transmisji adresowanej!");
            }
            try
            {
                await serialPort.OpenConnectionAsync(connectionSettings);
            }
            catch (Exception e)
            {
                errorOccuredHandlers(e.Message);
            }
        }
    }
}
