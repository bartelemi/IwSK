using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        #region Estabilishing connection

        /// <summary>
        /// Opens new serial port connection
        /// </summary>
        /// <param name="connectionSettings">Serial port connection settings</param>
        public void OpenConnection(ConnectionSettings connectionSettings)
        {
            try
            {
                if (IsOpen)
                    throw new UnauthorizedAccessException("Port is already opened.");

                _port.PortName = connectionSettings.PortName;
                _port.BaudRate = (int)(connectionSettings.BitRate);
                _port.DataBits = connectionSettings.CharacterFormat.DataFieldSize;
                _port.Parity = (Parity)connectionSettings.CharacterFormat.ParityControl;
                _port.StopBits = (StopBits)connectionSettings.CharacterFormat.StopBitsNumber;
                _port.NewLine = connectionSettings.TerminalString;

                _port.Open();
            }
            catch (InvalidOperationException invOpEx)
            {
                Debug.WriteLine(invOpEx.Message);
                if (IsOpen)
                {
                    throw new InvalidOperationException("Port jest już otwarty!");
                }
                else
                {
                    throw new InvalidOperationException("Port jest zamknięty!");    
                }
                
            }
            catch (TimeoutException tmoutEx)
            {
                Debug.WriteLine(tmoutEx.Message);
                throw new TimeoutException("Przekroczono czas połączenia!");
            }
            catch (UnauthorizedAccessException unAccEx)
            {
                Debug.WriteLine(unAccEx.Message);
                throw new UnauthorizedAccessException("Odmowa dostępu do portu!");
            }
            catch (EndOfStreamException eosEX)
            {
                Debug.WriteLine(eosEX.Message);
                throw new EndOfStreamException("Strumień danych zamknięty!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception("Nieznany błąd!");
            }
        }
        
        /// <summary>
        /// Automatically discovers connection settings of connection
        /// and estabilishes connection
        /// </summary>
        /// <param name="portName">Name of port</param>
        public void Autobaud(string portName)
        {
            var settings = new ConnectionSettings
            {
                PortName = portName
            };

            // TODO - get settings from selected port

            OpenConnection(settings);
        }

        #endregion Estabilishing connection

        #region Data exchange

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
                Debug.WriteLine(invOpEx.Message);
                throw new InvalidOperationException("Port jest zamknięty!");
            }
            catch (IOException ioEx)
            {
                Debug.WriteLine(ioEx.Message);
                throw new IOException("Ogólny błąd I/O!"); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new Exception("Nieznany błąd!");
            }
        }

        /// <summary>
        /// Sends message through serial port
        /// and waits for response
        /// </summary>
        /// <param name="messageProperties">Parameters of message</param>
        /// <param name="message">Message plain text</param>
        public void Transaction(MessageProperties messageProperties, string message)
        {
            SendMessage(messageProperties, message);
        }

        #endregion Data exchange

        #region Testing connection

        /// <summary>
        /// Tests connection and measures Round Trip Delay (RTD) time
        /// </summary>
        public void Ping()
        {
            // TODO
        }

        #endregion Testing connection

        #region Closing connection

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

        #endregion Closing connection

        #endregion Communication

        #region Helpers

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

        #endregion Helpers
    }
}