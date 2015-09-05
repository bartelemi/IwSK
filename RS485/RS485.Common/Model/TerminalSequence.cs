using RS232.Common.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS485.Serial.Model
{
    public class TerminalSequence : INotifyPropertyChanged
    {
        #region Fields

        private string _terminalString;
        private string _terminalStringVisible;
        private TerminatorType _terminatorType;
        private StringToVisibleASCIIConverter converter = new StringToVisibleASCIIConverter();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Text representation of terminal sequence
        /// </summary>
        public string TerminalString
        {
            get { return _terminalString; }
            private set
            {
                if (_terminalString != value)
                {
                    _terminalString = (value.Length > 2) 
                                    ? value.Substring(0, 2)
                                    : _terminalString = value;

                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Visible text representation of terminal sequence
        /// </summary>
        /// <remarks>
        /// All whitespaces and special characters are converted
        /// to visible text representation
        /// </remarks>
        public string TerminalStringVisible
        {
            get { return _terminalStringVisible; }
            set
            {
                if (value != _terminalStringVisible)
                {
                    TerminalString = converter.ConvertBack(value, 2);
                    _terminalStringVisible = converter.Convert(TerminalString);
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Type of terminal sequence
        /// </summary>
        public TerminatorType TerminatorType
        {
            get { return _terminatorType; }
            set
            {
                if (value != _terminatorType)
                {
                    _terminatorType = value;
                    RaisePropertyChanged();
                    ConvertDefault(); 
                }
            }
        }

        #endregion Properties

        #region Helpers

        /// <summary>
        /// Converts default values of TerminatorTypes to string
        /// </summary>
        protected void ConvertDefault()
        {
            switch (TerminatorType)
            {
                case TerminatorType.CR:
                    TerminalStringVisible = @"\13";
                    break;
                case TerminatorType.LF:
                    TerminalStringVisible = @"\10";
                    break;
                case TerminatorType.CRLF:
                    TerminalStringVisible = @"\13 \10";
                    break;
                default:
                    TerminalStringVisible = string.Empty;
                    break;
            }
        }

        #endregion Helpers

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion INotifyPropertyChanged
    }
}