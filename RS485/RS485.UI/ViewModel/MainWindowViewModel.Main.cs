using System;
using System.Collections.Generic;
using Moq;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Interfaces;
using RS485.Common.Model;
using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
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
            DisposeMaster();

            // TODO uncomment & fill
            //ModbusMaster = new ModbusMaster(...);

            InitializeMasterEvents();
        }

        private void ConnectSlave()
        {
            DisposeSlave();

            // TODO uncomment & fill
            //ModbusSlave = new ModbusSlave(...);

            InitializeSlaveEvents();
        }

        private void ExecuteModbusRequest()
        {
            // TODO uncomment 
            //switch (CommandMode)
            //{
            //    case CommandMode.One:
            //        ModbusMaster.SendFirstCommand(SlaveStationAddress.ToString(), CommandOneArguments);
            //        break;
            //    case CommandMode.Two:
            //        ModbusMaster.SendSecondCommand(SlaveStationAddress.ToString());
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        private void StartSlaveListener()
        {
            // TODO uncomment
            //ModbusSlave.StartListening();
        }

        private void LogMessageReceived(LogMessageOccuredEventArgs args)
        {
            OutputTextBoxContent += args.ToReadableLogMessage();
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