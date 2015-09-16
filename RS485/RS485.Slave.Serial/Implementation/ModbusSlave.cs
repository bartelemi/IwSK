using System;
using RS485.Common.Interfaces;
using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Model;
using RS485.Common.Serial;
using System.Diagnostics;
using RS485.Common.Implementation;

namespace RS485.Slave.Serial.Implementation
{
    public class ModbusSlave : IModbusSlave
    {
        public event ReceivedMessageFromMasterInFirstCommand FirstCommandReceived;
        public event SentTextToMasterInSecondCommand SecondCommandResponseSent;
        public event LogMessageOccuredEventHandler LogMessageOccured;

        private SlaveConfiguration _slaveConfig;
        private ConnectionSettings _connectionSettings;
        private readonly SerialPortHandler _serialPort = new SerialPortHandler();

        public ModbusSlave(ConnectionSettings settings, SlaveConfiguration config)
        {
            _connectionSettings = settings;
            _slaveConfig = config;
            _serialPort.OnDataReceived += SerialDataReceived;

        }

        public void SetConnectionSettings(ConnectionSettings settings)
        {
             _connectionSettings = settings;
        }

        public void SetSlaveConfiguration(SlaveConfiguration config)
        {
            _slaveConfig = config;
        }

        public void StartListening()
        {
            try
            {
                _serialPort.OpenConnectionAsync(_connectionSettings);
            }
            catch (Exception e)
            {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                throw e;
            }
            Debug.WriteLine("Slave port opened");
        }

        private void OnLogMessageOccured(LogMessageType logMsgType, string content)
        {
            if (LogMessageOccured != null)
                LogMessageOccured(new LogMessageOccuredEventArgs(logMsgType, content));
        }



        public void SerialDataReceived(string data, MessageType type)
        {
            Frame frame;
            try {
                frame = FrameBuilder.mapFromString(data);
            } catch (Exception e) {
                OnLogMessageOccured(LogMessageType.Error, e.Message);
                return;
            }
            Debug.WriteLine("Received message in slave: " + frame.Message);
            LogMessageOccured(new LogMessageOccuredEventArgs(LogMessageType.Info, "Hex frame: " + frame.getHexForm()));
            switch(dispatchOperation(frame)) {
                case SlavePossibleStates.COMMAND_1 :
                    command1Handler(frame);
                    break;
                case SlavePossibleStates.COMMAND_2 :
                    command2Handler(frame);
                    break;
            }
        }

        private SlavePossibleStates dispatchOperation(Frame frame)
        {
            if (frame.Message.StartsWith("1"))
            {
                return SlavePossibleStates.COMMAND_1;
            }
            else
            {
                return SlavePossibleStates.COMMAND_2;
            }
        }

        private async void command1Handler(Frame frame)
        {
            if (frame.DeviceAddress.Equals("000") || frame.DeviceAddress.Equals(Frame.normalizeAddress(_slaveConfig.SlaveAddress.ToString())))
            {
                await _serialPort.SendMessageAsync(CommandResult.Success.ToString());
                FirstCommandReceived(frame.Message.Substring(1));
            }
        }

        private void command2Handler(Frame frame)
        {
            throw new NotImplementedException();
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

    public enum SlavePossibleStates {
        COMMAND_1,
        COMMAND_2,
        WRONG_ADDRESS
    }
}
