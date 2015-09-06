namespace RS485.Common.Model
{
    /// <summary>
    /// Specifies format of characters to send
    /// </summary>
    public class CharacterFormat
    {
        #region Fields

        private int _dataFieldSize;
        private ParityControl _parityControl;
        private StopBitsNumber _stopBitsNumber;

        #endregion Fields

        /// <summary>
        /// Size of data field in bits.
        /// Allowed values are 7 bits or 8 bits.
        /// </summary>
        public int DataFieldSize
        {
            get { return _dataFieldSize; }
            set
            {
                if (value < 7)
                    value = 7;
                else if (value > 8)
                    value = 8;

                _dataFieldSize = value;
            }
        }

        /// <summary>
        /// Type of transmission control
        /// </summary>
        public ParityControl ParityControl
        {
            get { return _parityControl; }
            set { _parityControl = value; }
        }

        /// <summary>
        /// Number of Stop bits after every character
        /// </summary>
        public StopBitsNumber StopBitsNumber
        {
            get { return _stopBitsNumber; }
            set { _stopBitsNumber = value; }
        }
    }
}