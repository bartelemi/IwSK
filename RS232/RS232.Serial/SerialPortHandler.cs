using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS232.Serial.Model;

namespace RS232.Serial
{
    /// <summary>
    /// Handles communication through serial port
    /// </summary>
    public class SerialPortHandler
    {
        #region Fields

        private SerialPort _port = new SerialPort();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Indicates if serial port is open
        /// </summary>
        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }

        #endregion Properties

        #region Communication

        /// <summary>
        /// Creates message to send through serial port
        /// </summary>
        /// <param name="message">Text message to send</param>
        /// <param name="properties">Parameters of message</param>
        /// <returns>Fully configured message</returns>
        private string CreateMessage(string message, MessageProperties properties)
        {
            var sb = new StringBuilder();

            #region Append date time

            if (properties.AppendDateTime)
            {
                sb.Append(string.Format("{0:dd/MM/yy HH:mm:ss} > ", DateTime.Now));
            }

            #endregion Append date time

            sb.Append(message);

            #region Append terminator

            switch (properties.Terminator)
            {
                case Terminator.CR:
                    sb.Append('\r');
                    break;
                case Terminator.LF:
                    sb.Append('\n');
                    break;
                case Terminator.Custom:
                    if (string.IsNullOrEmpty(properties.CustomTerminator))
                    {
                        sb.Append('\r');
                        sb.Append('\n');
                    }
                    else
                    {
                        sb.Append(properties.CustomTerminator);
                    }
                    break;
                case Terminator.CRLF:
                    sb.Append('\r');
                    sb.Append('\n');
                    break;
                default:
                    sb.Append('\r');
                    sb.Append('\n');
                    break;
            }

            #endregion Append terminator

            return sb.ToString();
        }
        
        /// <summary>
        /// Opens new serial port connection
        /// </summary>
        /// <param name="connectionSettings">Serial port connection settings</param>
        /// <returns>True if port is opened successfully</returns>
        public bool OpenConnection(ConnectionSettings connectionSettings)
        {
            try
            {
                if (IsOpen)
                    throw new UnauthorizedAccessException();

                _port.PortName = connectionSettings.PortName;
                _port.BaudRate = (int)(connectionSettings.BitRate);
                _port.DataBits = connectionSettings.CharacterFormat.DataFieldSize;
                _port.Parity = (Parity)connectionSettings.CharacterFormat.ParityControl;
                _port.StopBits = (StopBits)connectionSettings.CharacterFormat.StopBitsNumber;
                _port.NewLine = connectionSettings.TerminalString;

                _port.Open();
                return true;
            }
            catch (InvalidOperationException invOpEx)
            {
                Console.WriteLine("Exception: Port isn't open.");
                throw;
            }
            catch (TimeoutException tmoutEx)
            {
                Console.WriteLine("Exception: Timeout.");
                throw;
            }
            catch (UnauthorizedAccessException unAccEx)
            {
                Console.WriteLine("Exception: Access to port denied.");
                throw;
            }
            catch (EndOfStreamException eosEX)
            {
                Console.WriteLine("Exception: End of stream.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: Unknown error.");
                throw;
            }
        }

        /// <summary>
        /// Sends message through serial port
        /// </summary>
        /// <param name="messageProperties">Parameters of message</param>
        /// <param name="message">Message plain text</param>
        public void SendMessage(MessageProperties messageProperties, string message)
        {
            try
            {
                var preparedMessage = CreateMessage(message, messageProperties);

                _port.DiscardOutBuffer();
                _port.DiscardInBuffer();

                _port.Write(preparedMessage);

                _port.DiscardOutBuffer();
                _port.DiscardInBuffer();
            }
            catch (InvalidOperationException invOpEx)
            {
                Console.WriteLine("Exception: Port isn't open.");
                throw;
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("Exception: I/O error.");
                throw;
            }
        }

        /// <summary>
        /// Closes connection
        /// </summary>
        public void CloseConnection()
        {
            if (IsOpen)
            {
                _port.DiscardOutBuffer();
                _port.DiscardInBuffer();
                _port.Close();
            }
        }

        #endregion Communication
    }
}