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
            set { _portNumber = value; }
        }

        /// <summary>
        /// Transmission speed in bits per second
        /// </summary>
        public BitRate BitRate
        {
            get { return _bitRate; }
            set { _bitRate = value; }
        }

        /// <summary>
        /// Message terminator
        /// </summary>
        public Terminator Terminator
        {
            get { return _terminator; }
            set { _terminator = value; }
        }

        /// <summary>
        /// Type of communication flow control
        /// </summary>
        public FlowControl FlowControl
        {
            get { return _flowControl; }
            set { _flowControl = value; }
        }

        /// <summary>
        /// Type of transmission (text/binary)
        /// </summary>
        public Transmission Transmission
        {
            get { return _transmission; }
            set { _transmission = value; }
        }

        /// <summary>
        /// Format of single character to send
        /// </summary>
        public CharacterFormat CharacterFormat
        {
            get { return _characterFormat; }
            set { _characterFormat = value; }
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