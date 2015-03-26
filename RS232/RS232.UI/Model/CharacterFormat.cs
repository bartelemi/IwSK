using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS232.UI.Model
{
    /// <summary>
    /// Specifies format of characters to send
    /// </summary>
    public class CharacterFormat
    {
        #region Fields

        private int _stopBits;
        private int _dataFieldSize;
        private TransmissionControl _controlType;

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
        /// Number of stop bits.
        /// Allowed values are 1 or 2.
        /// </summary>
        public int StopBits 
        {
            get { return _stopBits; }
            set
            {
                if (value < 1)
                    value = 1;
                if (value > 2)
                    value = 2;

                _stopBits = value;
            }
        }

        /// <summary>
        /// Type of transmission control
        /// </summary>
        public TransmissionControl ControlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }
    }
}
