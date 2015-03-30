using System;
using System.Collections.Generic;
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
        public string CreateMessage(string message, MessageProperties properties)
        {
            StringBuilder sb = new StringBuilder();

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

        public bool Connect()
        {
            SerialPort serialPort = new SerialPort();
            serialPort.BaudRate = 1;
            serialPort.DataBits = 8;
            serialPort.Handshake = Handshake.None;
            serialPort.StopBits = StopBits.Two;

            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {
                
            }
            return false;
        }
    }
}