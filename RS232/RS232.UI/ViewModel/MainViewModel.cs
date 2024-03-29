using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using GalaSoft.MvvmLight;
using RS232.Serial;
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
        private int _readTimeout;
        private int _writeTimeout; 
        private string _statusText;
        private string _messageText;
        private InputType _inputType;
        private bool _appendDateTime;
        private string _selectedPortName;
        private FlowControl _flowControl;
        private Encoding _serialPortEncoding;
        private CharacterFormat _characterFormat;
        private ConnectionState _connectionState;
        private TerminalSequence _terminalSequence;
        private StringBuilder _receivedMessages = new StringBuilder();
        private SerialPortHandler _serialPortHandler = new SerialPortHandler();

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
        /// Number of milliseconds before a time-out
        /// occurs when a read operation does not finish.
        /// </summary>
        public int ReadTimeout
        {
            get { return _readTimeout; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 15000)
                {
                    value = System.IO.Ports.SerialPort.InfiniteTimeout;
                }
                if (value != _readTimeout)
                {
                    _readTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Number of milliseconds before a time-out 
        /// occurs when a write operation does not finish.
        /// </summary>
        public int WriteTimeout
        {
            get { return _writeTimeout; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 15000)
                {
                    value = System.IO.Ports.SerialPort.InfiniteTimeout;
                }
                if (value != _writeTimeout)
                {
                    _writeTimeout = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Current status description
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
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
        /// Returns collection of serial ports names
        /// </summary>
        public string[] SerialPortNames
        {
            get { return System.IO.Ports.SerialPort.GetPortNames(); }
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
        /// Selected encoding for serial port
        /// </summary>
        public Encoding SerialPortEncoding
        {
            get { return _serialPortEncoding; }
            set
            {
                if (_serialPortEncoding != value)
                {
                    _serialPortEncoding = value;
                    RaisePropertyChanged();
                }
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
        /// Messages received from serial port
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

        /// <summary>
        /// Terminal sequence wrapper for terminal string of messages
        /// </summary>
        public TerminalSequence TerminalSequence
        {
            get { return _terminalSequence; }
            set
            {
                _terminalSequence = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Available character encodings for SerialPort
        /// </summary>
        public Encoding[] SerialPortEncodings
        {
            get
            {
                return new[]
                {
                    Encoding.ASCII, 
                    Encoding.UTF8,
                    Encoding.UTF32,
                    Encoding.Unicode
                };
            }
        }

        /// <summary>
        /// Reference to serial port handler
        /// </summary>
        public SerialPortHandler SerialPortHandler
        {
            get { return _serialPortHandler; }
        }
        #endregion Properties

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitSerialPortSettings();
            InitCommands();

            _serialPortHandler.OnDataReceived += new SerialPortHandler.DataReceived(SerialPortDataReceived);
        }

        /// <summary>
        /// Assign default values to connection properties
        /// </summary>
        private void InitSerialPortSettings()
        {
            AppendDateTime = false;

            BitRate = BitRate.BR_115200;
            ReadTimeout = 500;
            WriteTimeout = 500;
            SerialPortEncoding = Encoding.ASCII;
            TerminalSequence = new TerminalSequence
            {
                TerminatorType = TerminatorType.CRLF 
            };
            FlowControl = FlowControl.None;
            InputType = InputType.Text;
            CharacterFormat = new CharacterFormat
            {
                DataFieldSize = 8,
                ParityControl = ParityControl.None,
                StopBitsNumber = StopBitsNumber.One
            };

            ConnectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        /// Sets properties to the given values
        /// </summary>
        /// <param name="settings">Serial port settings</param>
        private void SetSerialPortSettings(ConnectionSettings settings)
        {
            TerminatorType terminator;
            if (TerminatorType.TryParse(settings.TerminalString, true, out terminator))
            {
                TerminalSequence.TerminatorType = terminator;
            }
            else
            {
                TerminalSequence.TerminatorType = TerminatorType.Custom;
                TerminalSequence.TerminalStringVisible = settings.TerminalString;
            }

            BitRate = settings.BitRate;
            FlowControl = settings.FlowControl;
            CharacterFormat = new CharacterFormat
            {
                DataFieldSize = settings.CharacterFormat.DataFieldSize,
                ParityControl = settings.CharacterFormat.ParityControl,
                StopBitsNumber = settings.CharacterFormat.StopBitsNumber
            };
        }

        #endregion Initialization

        #region Serial port events

        /// <summary>
        /// Updates viewModel according to changes in serial port state
        /// </summary
        /// <param name="data">Data received</param>
        private void SerialPortDataReceived(string data, MessageType type)
        {
            switch (type)
            {
                case MessageType.Plain:
                    ReceivedMessages = data;
                    break;
                case MessageType.TransactionBegin:
                    //TODO: Handle transaction begin
                    break;
                default:
                    break;
            }
        }
        #endregion Serial port events
    }
}