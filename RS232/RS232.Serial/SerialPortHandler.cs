using System;
using System.Collections.Generic;
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

        private void Connect(ConnectionSettings connectionSettings, SerialPort port)
        {
            if(port.IsOpen)
                throw new UnauthorizedAccessException();

            port.PortName = connectionSettings.PortName;
            port.BaudRate = (int)(connectionSettings.BitRate);
            port.DataBits = connectionSettings.DataBits;
            port.Parity = (Parity)connectionSettings.ParityControl;

            port.StopBits = (StopBits)connectionSettings.CharacterFormat.StopBitsNumber;
            //port.Parity = connectionSettings.CharacterFormat;

            port.Open();
        }

        /// <summary>
        /// Sends message through serial port
        /// </summary>
        /// <param name="connectionSettings">Settings for connection</param>
        /// <param name="messageProperties">Parameters of message</param>
        /// <param name="message">Message plain text</param>
        public void SendMessage(ConnectionSettings connectionSettings, MessageProperties messageProperties, string message)
        {
            using (var serialPort = new SerialPort())
            {
                try
                {
                    Connect(connectionSettings, serialPort);
                    var preparedMessage = CreateMessage(message, messageProperties);

                    serialPort.DiscardOutBuffer();
                    serialPort.DiscardInBuffer();

                    serialPort.Write(preparedMessage);

                    serialPort.DiscardOutBuffer();
                    serialPort.DiscardInBuffer();
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
        }
    }
}