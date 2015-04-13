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
        private string _statusText;
        private string _messageText;
        private InputType _inputType;
        private bool _appendDateTime;
        private string _selectedPortName;
        private FlowControl _flowControl;
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

        public TerminalSequence TerminalSequence
        {
            get { return _terminalSequence; }
            set
            {
                _terminalSequence = value;
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
            InitSerialPortSettings();
            InitCommands();

            _serialPortHandler.PropertyChanged += new PropertyChangedEventHandler(SerialPortHandler_StateChanged);
        }

        /// <summary>
        /// Assign default values to connection properties
        /// </summary>
        private void InitSerialPortSettings()
        {
            AppendDateTime = false;

            BitRate = BitRate.BR_115200;
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
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void SerialPortHandler_StateChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsDSRActive":
                {
                    break;
                }
                case "IsCTSActive":
                {
                    break;
                }
                case "ReceivedData":
                {
                    var data = _serialPortHandler.ReceivedData;
                    if (!string.IsNullOrEmpty(data))
                        ReceivedMessages = data;
                    break;
                }
            }
        }

        #endregion Serial port events
    }
}