using GalaSoft.MvvmLight;
using RS485.UI.Model;

namespace RS485.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region consts

        private const string WindowTitleCommon = "RS-485 - MODBUS ({0})";

        #endregion consts

        #region Fields

        private Station _station;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Type of current station
        /// </summary>
        public Station Station
        {
            get { return _station; }
            set
            {
                if (_station != value)
                _station = value;

                RaisePropertyChanged("WindowTitle");
            }
        }

        /// <summary>
        /// Title of main window
        /// </summary>
        public string WindowTitle
        {
            get
            {
                return string.Format(WindowTitleCommon, Station.ToString()); ;
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
            Station = Station.Master;
        }

        #endregion Initialization
    }
}