using System.Text;
using GalaSoft.MvvmLight;
using RS232.Serial.Model;

namespace RS232.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        #region Fields

        
        private int _portNumber;
        private BitRate _bitRate;
        private string _messageText;
        private InputType _inputType; 
        private bool _appendDateTime; 
        private Terminator _terminator;
        private string _customTerminator;
        private string _selectedPortName;
        private FlowControl _flowControl; 
        private CharacterFormat _characterFormat;
        private ConnectionState _connectionState;
        private StringBuilder _receivedMessages = new StringBuilder();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Number of port for connection
        /// </summary>
        public int PortNumber
        {
            get { return _portNumber; }
            set
            {
                _portNumber = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Transmission speed in bits per second
        /// </summary>
        public BitRate BitRate
        {
            get { return _bitRate; }
            set 
            { 
                _bitRate = value; 
                RaisePropertyChanged(); 
            }
        }

        /// <summary>
        /// Text of message to be send
        /// </summary>
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Input type for messages (text/binary)
        /// </summary>
        public InputType InputType
        {
            get { return _inputType; }
            set
            {
                _inputType = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Defines if date and time should added at the begining of the message
        /// </summary>
        public bool AppendDateTime
        {
            get { return _appendDateTime; }
            set
            {
                _appendDateTime = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Message terminator
        /// </summary>
        public Terminator Terminator
        {
            get { return _terminator; }
            set
            {
                _terminator = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Returns collection of serial ports names
        /// </summary>
        public string[] SerialPortNames
        {
            get { return System.IO.Ports.SerialPort.GetPortNames(); }
        }

        /// <summary>
        /// Custom terminator text
        /// </summary>
        public string CustomTerminator
        {
            get { return _customTerminator; }
            set
            {
                _customTerminator = value.Substring(0, 2);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Holds name of selected serial port
        /// </summary>
        public string SelectedPortName
        {
            get { return _selectedPortName; }
            set
            {
                _selectedPortName = value; 
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// Type of communication flow control
        /// </summary>
        public FlowControl FlowControl
        {
            get { return _flowControl; }
            set
            {
                _flowControl = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Format of single character to send
        /// </summary>
        public CharacterFormat CharacterFormat
        {
            get { return _characterFormat; }
            set
            {
                _characterFormat = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Text of message to be send
        /// </summary>
        public string ReceivedMessages
        {
            get { return _receivedMessages.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _receivedMessages.Append(value);
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Current state of connection
        /// </summary>
        public ConnectionState ConnectionState
        {
            get { return _connectionState; }
            set
            {
                _connectionState = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitProperties();
            InitCommands();
        }

        /// <summary>
        /// Assign default values to connection properties
        /// </summary>
        private void InitProperties()
        {
            AppendDateTime = false;

            BitRate = BitRate.BR_115200;
            Terminator = Terminator.CRLF;
            FlowControl = FlowControl.None;
            InputType = InputType.Text;
            CharacterFormat = new CharacterFormat
            {
                DataFieldSize = 8,
                ControlType = ParityControl.None,
                StopBitsNumber = StopBitsNumber.Zero
            };

            ConnectionState = ConnectionState.Disconnected;
        }

        #endregion Initialization
    }
}