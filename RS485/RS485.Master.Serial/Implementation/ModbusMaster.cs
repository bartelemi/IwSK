using RS485.Common.Serial;
using RS485.Common.Model;
using RS485.Common.Implementation;
using RS485.Master.Serial.Exceptions;
using RS485.Master.Serial.Model;
using System;
using System.Threading;
using System.Diagnostics;
using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Interfaces;

namespace RS485.Master.Serial.Implementation
{
    public class ModbusMaster : IModbusMaster
    {
        public event FirstCommandCompleted FirstCommandCompletedHandlers;
        public event SecondCommandCompleted SecondCommandCompletedHandlers;
        public event LogMessageOccuredEventHandler LogMessageOccured;

        private ModbusSettings _modbusSettings;
        private ConnectionSettings _connectionSettings;
        private MessageProperties _messageProperties;
        private readonly SerialPortHandler _serialPort = new SerialPortHandler();
        private Transaction _currentTransaction;
        private string _lastReceivedData = "";

        public void SetConnectionSettings(ConnectionSettings settings)
        {
            this._connectionSettings = settings;
        }

        public void SetModbusSettings(ModbusSettings settings)
        {
            this._modbusSettings = settings;
        }

        public ModbusMaster(ConnectionSettings connectionSettings, ModbusSettings modbusSettings)
        {
            this._connectionSettings = connectionSettings;
            this._modbusSettings = modbusSettings;
            this._serialPort.OnDataReceived += receivedDataHandler;

        }

        public async void SendFirstCommand(string slaveAddress, string message)
        {
            try
            {
                await _serialPort.OpenConnectionAsync(_connectionSettings);
                Debug.WriteLine("Port opened");
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                return;
            }
            int retransmissionsLeft = _modbusSettings.RetransmissionsCount;
            Frame frameToSend = FrameBuilder.buildFrame(slaveAddress, message);
            Debug.WriteLine("Frame builded: " + frameToSend);
            do
            {
                _currentTransaction = new Transaction(_modbusSettings.TransactionDuration);
                try
                {
                    SendFrame(frameToSend);
                }
                catch (Exception e)
                {
                    OnLogMessageOccured(LogMessageType.Error, e.Message);
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                    return;
                }
                while (_currentTransaction.isTransactionStillActive())
                {
                    Debug.WriteLine("Transaction still active, wait for receive");
                    if (IsFrameSendSuccess())
                    {
                        Debug.WriteLine("Received some data: " + _lastReceivedData);
                        if (_lastReceivedData.Equals(CommandResult.SUCCESS))
                        {
                            Debug.WriteLine("Tranmission success!");
                            SendFirstCommandCompletedEventSuccess();
                            return;
                        }
                        else
                        {
                            Debug.WriteLine("Tranmission fail, wrong ACK!");
                            SendFirstCommandCompletedEventFail();
                            return;
                        }
                    }
                    Thread.Sleep(5);
                }
                Debug.WriteLine("transaction timeout, decrease retransmission");
                retransmissionsLeft--;

            } while (retransmissionsLeft > 0);
            Debug.WriteLine("Communication fail, no ACK received");
            SendFirstCommandCompletedEventFail();
        }

        private void SendFirstCommandCompletedEventFail()
        {
            _lastReceivedData = "";
            FirstCommandCompletedHandlers(CommandResult.FAIL);
        }

        private void SendFirstCommandCompletedEventSuccess()
        {
            _lastReceivedData = "";
            FirstCommandCompletedHandlers(CommandResult.SUCCESS);
        }

        private void SendFrame(Frame frameToSend)
        {
            _serialPort.SendMessageAsync(frameToSend.getStringToSend());
        }

        private bool IsFrameSendSuccess()
        {
            return !_lastReceivedData.Equals("ERROR");
        }

        private void receivedDataHandler(string data, MessageType type)
        {
            this._lastReceivedData = data;
        }

        public async void SendSecondCommand(string slaveAddress)
        {
            if ("0".Equals(slaveAddress))
            {
                throw new InvalidArgumentException("Operacja nr. 2 może być realizowana tylko dla transmisji adresowanej!");
            }
            try
            {
                await _serialPort.OpenConnectionAsync(_connectionSettings);
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
            }
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }
    }
}
