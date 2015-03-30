using GalaSoft.MvvmLight;
using RS232.UI.Model;

namespace RS232.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private int _portNumber;
        private BitRate _bitRate;
        private Terminator _terminator;
        private string _customTerminator;
        private string _selectedPortName;
        private FlowControl _flowControl;
        private Transmission _transmission;
        private CharacterFormat _characterFormat;

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
        /// Type of transmission (text/binary)
        /// </summary>
        public Transmission Transmission
        {
            get { return _transmission; }
            set
            {
                _transmission = value;
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

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            InitProperties();
        }

        /// <summary>
        /// Assign default values to connection properties
        /// </summary>
        private void InitProperties()
        {
            BitRate = BitRate.BR_115200;
            Terminator = Terminator.CRLF;
            FlowControl = FlowControl.None;
            Transmission = Transmission.Text;
            CharacterFormat = new CharacterFormat
            {
                StopBits = 2,
                DataFieldSize = 8,
                ControlType = TransmissionControl.None
            };
        }

        #endregion Initialization
    }
}