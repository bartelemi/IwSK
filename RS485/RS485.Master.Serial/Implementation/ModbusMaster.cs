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
using RS485.Common.Exceptions;

namespace RS485.Master.Serial.Implementation
{
    public class ModbusMaster : IModbusMaster
    {
        public event FirstCommandCompletedEventHandler FirstCommandCompleted;
        public event SecondCommandCompletedEventHandler SecondCommandCompleted;
        public event LogMessageOccuredEventHandler LogMessageOccured;

        private ModbusSettings _modbusSettings;
        private ConnectionSettings _connectionSettings;
        private readonly SerialPortHandler _serialPort = new SerialPortHandler();
        private Transaction _currentTransaction;
        private string _lastReceivedData = "";


        public void SetConnectionSettings(ConnectionSettings settings)
        {
            _connectionSettings = settings;
        }

        public void SetModbusSettings(ModbusSettings settings)
        {
            _modbusSettings = settings;
        }

        public ModbusMaster(ConnectionSettings connectionSettings, ModbusSettings modbusSettings)
        {
            _connectionSettings = connectionSettings;
            _modbusSettings = modbusSettings;
            _serialPort.OnDataReceived += receivedDataHandler;
            try
            {
                _serialPort.OpenConnectionAsync(_connectionSettings);
                Debug.WriteLine("Master: Port opened");
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                throw e;
            }

        }

        public void SendFirstCommand(string slaveAddress, string message)
        {
            int retransmissionsLeft = _modbusSettings.RetransmissionsCount;
            Frame frameToSend = FrameBuilder.buildFrame(slaveAddress, "1"+message);

            if (slaveAddress.Equals("000"))
            {
                SendFrame(frameToSend);
                SendFirstCommandCompletedEventSuccess();
            }
            else
            {
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
                        Debug.WriteLine("Master: Transaction still active, wait for receive");
                        if (IsFrameSendSuccessFirstCommand())
                        {
                            Debug.WriteLine("Master: Received some data: " + _lastReceivedData);
                            if (_lastReceivedData.Equals(CommandResult.Success.ToString()))
                            {
                                Debug.WriteLine("MAster: Transmission success!");
                                SendFirstCommandCompletedEventSuccess();
                                return;
                            }

                            Debug.WriteLine("Master: Transmission fail, wrong ACK!");
                            SendFirstCommandCompletedEventFail();
                            return;
                        }
                        Thread.Sleep(5);
                    }
                    Debug.WriteLine("Master: transaction timeout, decrease retransmission");
                    retransmissionsLeft--;

                } while (retransmissionsLeft > 0);

                Debug.WriteLine("Master: Communication fail, no ACK received");
                SendFirstCommandCompletedEventFail();
            }
        }

        private void SendFirstCommandCompletedEventFail()
        {
            _lastReceivedData = "";
            FirstCommandCompleted(CommandResult.Fail);
        }

        private void SendFirstCommandCompletedEventSuccess()
        {
            _lastReceivedData = "";
            FirstCommandCompleted(CommandResult.Success);
        }

        private  void SendFrame(Frame frameToSend)
        {

            try
            {
                _serialPort.SendMessageAsync(frameToSend.getStringToSend());
            }
            catch (InternalErrorException ex)
            {
                OnLogMessageOccured(LogMessageType.Error, ex.Message);
            }
            
        }

        private bool IsFrameSendSuccessFirstCommand()
        {
            return (!_lastReceivedData.Equals("ERROR")) & _lastReceivedData.Length > 1;
        }

        private void receivedDataHandler(string data, MessageType type)
        {
            _lastReceivedData = data;
        }

        public async void SendSecondCommand(string slaveAddress)
        {
            if ("0".Equals(slaveAddress))
            {
                throw new InvalidArgumentException("Operacja nr. 2 może być realizowana tylko dla transmisji adresowanej!");
            }

            int retransmissionsLeft = _modbusSettings.RetransmissionsCount;
            Frame frameToSend = FrameBuilder.buildFrame(slaveAddress, "2");

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
                        Debug.WriteLine("Master: Transaction still active, wait for receive");
                        if (isTransactionSuccessSecondCommand())
                        {
                            Debug.WriteLine("Master: Received some data: " + _lastReceivedData);
                            Frame receivedFrame = FrameBuilder.mapFromString(_lastReceivedData);
                            SecondCommandCompleted(CommandResult.Success, receivedFrame.Message);
                            return;
                        }
                        Thread.Sleep(5);
                    }
                    Debug.WriteLine("Master: transaction timeout, decrease retransmission");
                    retransmissionsLeft--;

                } while (retransmissionsLeft > 0);

                Debug.WriteLine("Master: Communication fail, no response");
                SecondCommandCompleted(CommandResult.Fail, "");
        }

        private bool isTransactionSuccessSecondCommand()
        {
            try
            {
                Frame frame = FrameBuilder.mapFromString(_lastReceivedData);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }

        private void ClosePort()
        {
            _serialPort.CloseConnectionAsync();
        }

        public void Dispose()
        {
            ClosePort();
        }
    }
}
