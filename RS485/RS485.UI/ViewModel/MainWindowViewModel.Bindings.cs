using RS485.Common.Model;
using RS485.Common.Converters;
using RS485.UI.Helpers;
using RS485.Common.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            InitSerialPortSettings();
            // InitCommands();

            //  _serialPortHandler.OnDataReceived += new SerialPortHandler.DataReceived(SerialPortDataReceived);
        }

        public void InitSerialPortSettings()
        {
            _commandMode = CommandMode.One;
            _masterSlave = MasterSlave.Master;
            _transactionType = TransactionType.Broadcast;
            _slaveStationAddress = 123;
            _timeoutRetransmission = 500;
            _timeoutTransmission = 500;
            _numberOfRetransmission = 2;

        }

        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }
        private bool _isProcessing;

        public object OutputTextBoxContent
        {
            get { return _outputTextBoxContent; }
            set
            {
                _outputTextBoxContent = value; 
                OnPropertyChanged();
            }
        }
        private object _outputTextBoxContent;

        public string TestBindProperty
        {
            get { return _testBindProperty; }
            set
            {
                _testBindProperty = value;
                OnPropertyChanged();
            }
        }
        private string _testBindProperty;

         public string CommandOneArguments
        {
            get { return _commandOneArguments; }
            set
            {
                _commandOneArguments = value;
                OnPropertyChanged();

            }
        }
        private string _commandOneArguments;


        public string InputSlave
        {
            get { return _inputSlave; }
            set
            {
                _inputSlave = value;
                OnPropertyChanged();

            }
        }
        private string _inputSlave;


        /// <summary>
        /// Number of port for connection
        /// </summary>
        public int PortNumber
        {
            get { return _portNumber; }
            set
            {
                _portNumber = value;
                OnPropertyChanged();
            }
        }
        private int _portNumber;

        /// <summary>
        /// Holds name of selected serial port
        /// </summary>
        public string SelectedPortName
        {
            get { return _selectedPortName; }
            set
            {
                _selectedPortName = value;
                OnPropertyChanged();
            }
        }
        private string _selectedPortName;


        /// <summary>
        /// Current state of connection
        /// </summary>
        public ConnectionState ConnectionState
        {
            get { return _connectionState; }
            set
            {
                _connectionState = value;
                OnPropertyChanged();
            }
        }
        private ConnectionState _connectionState;

        /// <summary>
        /// Master or Slave mode
        /// </summary>
        public MasterSlave MasterSlave
        {
            get { return _masterSlave; }
            set
            {
                _masterSlave = value;
                OnPropertyChanged();
            }
        }
        private MasterSlave _masterSlave;

        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                OnPropertyChanged();
            }
        }
        private TransactionType _transactionType;

        public CommandMode CommandMode
        {
            get { return _commandMode; }
            set
            {
                _commandMode = value;
                OnPropertyChanged();
            }
        }
        private CommandMode _commandMode;

        public int SlaveStationAddress
        {
            get { return _slaveStationAddress; }
            set
            {
                _slaveStationAddress = value;
                OnPropertyChanged();
            }
        }
        private int _slaveStationAddress;

        public int TimeoutTransmission
        {
            get { return _timeoutTransmission; }
            set
            {
                _timeoutTransmission = value;
                OnPropertyChanged();
            }
        }
        private int _timeoutTransmission;

        public int TimeoutRetransmission
        {
            get { return _timeoutRetransmission; }
            set
            {
                _timeoutRetransmission = value;
                OnPropertyChanged();
            }
        }
        private int _timeoutRetransmission;

        public int NumberOfRetransmission
        {
            get { return _numberOfRetransmission; }
            set
            {
                _numberOfRetransmission = value;
                OnPropertyChanged();
            }
        }
        private int _numberOfRetransmission;

        /// <summary>
        /// Messages received from serial port
        /// </summary>
        public string OutputMaster
        {
            get { return _outputMaster.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _outputMaster.Append(value);
                    OnPropertyChanged();
                }
            }
        }
        private StringBuilder _outputMaster = new StringBuilder();

        public string OutputSlave
        {
            get { return _outputSlave.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _outputSlave.Append(value);
                    OnPropertyChanged();
                }
            }
        }
        private StringBuilder _outputSlave = new StringBuilder();

        public string TransmissionInfoOutput
        {
            get { return _transmissionInfoOutput.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _transmissionInfoOutput.Append(value);
                    OnPropertyChanged();
                }
            }
        }
        private StringBuilder _transmissionInfoOutput = new StringBuilder();

        public string ErrorOutputMaster
        {
            get { return _errorOutputMaster.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _errorOutputMaster.Append(value);
                    OnPropertyChanged();
                }
            }
        }
        private StringBuilder _errorOutputMaster = new StringBuilder();

        public string ErrorOutputSlave
        {
            get { return _errorOutputSlave.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _errorOutputSlave.Append(value);
                    OnPropertyChanged();
                }
            }
        }
        private StringBuilder _errorOutputSlave = new StringBuilder();

    }
    
}