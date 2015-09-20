using System;
using System.Collections.Generic;
using Moq;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.UI.Helpers;
using RS485.Master.Serial.Implementation;
using RS485.Slave.Serial.Implementation;
using System.Diagnostics;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public static ConnectionSettings SpawnConnectionSettings(string portName)
        {
            ConnectionSettings settings = new ConnectionSettings
            {
                BitRate = BitRate.BR_9600,
                CharacterFormat = new CharacterFormat
                {
                    DataFieldSize = 8,
                    ParityControl = ParityControl.Even,
                    StopBitsNumber = StopBitsNumber.One
                },
                FlowControl = FlowControl.Manual,
                PortName = portName,
                ReadTimeout = 500,
                WriteTimeout = -1,
                TerminalString = "###"
            };
            return settings;
        }

        public static ModbusSettings SpawnModbusSettings(int retransCount, int transDuration)
        {
            ModbusSettings modbus = new ModbusSettings
            {
                RetransmissionsCount = retransCount,
                TransactionDuration = transDuration
            };
            return modbus;
        }

        public static SlaveConfiguration SpawnSlaveConfig(int address, String textBackToMaster)
        {
            SlaveConfiguration config = new SlaveConfiguration
            {
                SlaveAddress = address,
                TextSendBackToMaster = textBackToMaster
            };
            return config;
        }
        public IModbusMaster ModbusMaster
        {
            get { return _modbusMaster; }
            private set
            {
                _modbusMaster = value; 
                OnPropertyChanged();
            }
        }
        private IModbusMaster _modbusMaster;

        public IModbusSlave ModbusSlave
        {
            get { return _modbusSlave; }
            private set
            {
                _modbusSlave = value;
                OnPropertyChanged();
            }
        }
        private IModbusSlave _modbusSlave;

        public MainWindowViewModel()
        {
            InitializeCommands();
            InitSerialPortSettings();
        }

        private void ConnectToModbus(MasterSlave connectMode)
        {
            switch (connectMode)
            {
                case MasterSlave.Master:
                    ConnectMaster();
                    break;
                case MasterSlave.Slave:
                    ConnectSlave();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("connectMode", connectMode, null);
            }
        }

        private void ConnectMaster()
        {
            if(ModbusMaster != null)
                DisposeMaster();
            ModbusMaster = new ModbusMaster(SpawnConnectionSettings(SelectedPortName), SpawnModbusSettings(NumberOfRetransmission, MaxDelayBetweenData));
            ConnectionState = Common.Model.ConnectionState.Connected;
           

            InitializeMasterEvents();
        }

        private void ConnectSlave()
        {
            if (ModbusSlave != null)
           DisposeSlave();

            ModbusSlave = new ModbusSlave(SpawnConnectionSettings(SelectedPortName), SpawnSlaveConfig(SlaveStationAddress, InputSlave));
            ConnectionState = Common.Model.ConnectionState.Connected;
            InitializeSlaveEvents();
        }

        private void ExecuteModbusRequest()
        {
            Debug.WriteLine("Send command!");
            switch (CommandMode)
            {
                case CommandMode.One:
                    ConnectionState = Common.Model.ConnectionState.Sending;
                    ModbusMaster.SendFirstCommand(SlaveStationAddress.ToString(), CommandOneArguments);
                    break;
                case CommandMode.Two:
                   ModbusMaster.SendSecondCommand(SlaveStationAddress.ToString());
                   break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartSlaveListener()
        {
            ConnectionState = Common.Model.ConnectionState.Sending;
            ModbusSlave.StartListening();
        }

        private void LogMessageReceived(LogMessageOccuredEventArgs args)
        {
            OutputTextBoxContent += args.ToReadableLogMessage() + "\n";
        }

        private void FirstCommandCompletedMaster(CommandResult result)
        {
            OutputTextBoxContent += "OPERATION STATUS: " + result.ToString() + "\n";
            ConnectionState = Common.Model.ConnectionState.Connected;
        }

        private void FirstCommandReceivedSlave(String message)
        {
            OutputTextBoxContent += "RECEIVED FROM MASTER: " + message.ToString() + "\n";
            OutputSlave = message;
        }

        private void SecondCommandCompletedMaster(CommandResult result, String message)
        {
            OutputTextBoxContent += "OPERATION STATUS: " + result.ToString() + "\n";
            if (result == CommandResult.Success)
            {
                OutputTextBoxContent += "Received from slave: " + message + "\n";
                OutputMaster = message;
            }
            ConnectionState = Common.Model.ConnectionState.Connected;
        }

        private void SecondCommandCompletedSlave()
        {
            ConnectionState = Common.Model.ConnectionState.Connected;
            OutputTextBoxContent += "SENT DATA TO MASTER \n";
        }

        /// <summary>
        /// Returns collection of serial ports names
        /// </summary>
        public static IEnumerable<string> SerialPortNames
        {
            get { return System.IO.Ports.SerialPort.GetPortNames(); }
        }

        #region Dispose area

        public override void Dispose()
        {
            DisposeMaster();
            DisposeSlave();

            base.Dispose();
        }

        #endregion
    }
}