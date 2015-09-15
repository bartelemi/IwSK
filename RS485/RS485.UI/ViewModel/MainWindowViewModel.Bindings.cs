using RS485.Common.Model;
using System.Text;
using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        private void InitSerialPortSettings()
        {
            _commandMode = CommandMode.One;
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

        public TransactionType TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;
                OnPropertyChanged();

                if (value != TransactionType.Address)
                {
                    CommandMode = CommandMode.One;
                }
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
            get { return _outputMaster; }
            set
            {
                _outputMaster = value;
                OnPropertyChanged();
            }
        }
        private string _outputMaster;

        public string OutputSlave
        {
            get { return _outputSlave; }
            set
            {
                _outputSlave = value;
                OnPropertyChanged();
            }
        }
        private string _outputSlave;
    }
    
}