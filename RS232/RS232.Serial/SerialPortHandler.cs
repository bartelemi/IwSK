﻿using System;
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
    public class SerialPortHandler : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Message recognized as ping request
        /// </summary>
        private const string PingMessage = "FFFF\r\n";

        /// <summary>
        /// Message send back when received ping request
        /// </summary>
        private const string PingResponse = "OK\r\n";

        private bool _isDSRActive;
        private bool _isCTSActive;
        private string _receivedData;
        private SerialPort _port = new SerialPort();
        private StringBuilder _incomingMessage = new StringBuilder();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Indicates if serial port is open
        /// </summary>
        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }

        /// <summary>
        /// Holds the state of DSR pin
        /// </summary>

        public bool IsDSRActive
        {
            get { return _isDSRActive; }
            set
            {
                _isDSRActive = value;
                RaisePropertyChanged("IsDSRActive");
            }
        }

        /// <summary>
        /// Holds the state of CTS pin
        /// </summary>
        public bool IsCTSActive
        {
            get { return _isCTSActive; }
            set
            {
                _isCTSActive = value;
                RaisePropertyChanged("IsCTSActive");
            }
        }

        /// <summary>
        /// Data read from serial port
        /// </summary>
        public string ReceivedData
        {
            get { return _receivedData; }
            set
            {
                _receivedData = value;
                RaisePropertyChanged("ReceivedData");
            }
        }

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Initializes instance of SerialPortHandler
        /// </summary>
        public SerialPortHandler()
        {
            _port.PinChanged += (o, e) =>
            {
                var serialPinChange = e.EventType;
                switch (serialPinChange)
                {
                    case SerialPinChange.CtsChanged:
                        IsCTSActive = !IsCTSActive;
                        break;
                    case SerialPinChange.DsrChanged:
                        IsDSRActive = !IsDSRActive;
                        break;
                }
            };

            _port.DataReceived += (o, e) =>
            {
                var text = _port.ReadExisting();



                if (!string.IsNullOrEmpty(text))
                {
                    // Check if it is ping request
                    if (text.Equals(PingMessage))
                    {
                        SendMessage(new MessageProperties(), PingResponse);
                    }
                    else
                    {
                        _incomingMessage.Append(text);
                        if (text.Contains(_port.NewLine))
                        {
                            ReceivedData = _incomingMessage.ToString();
                            _incomingMessage.Clear();
                        }
                    }
                }
            };
        }

        #endregion Initialization

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

            // TODO: get settings from selected port

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
        /// <returns>Response message</returns>
        public string Transaction(MessageProperties messageProperties, string message)
        {
            SendMessage(messageProperties, message);

            // TODO: wait for response
            //       return received value

            return string.Empty;
        }

        #endregion Data exchange

        #region Testing connection

        /// <summary>
        /// Tests connection and measures Round Trip Delay (RTD) time
        /// </summary>
        public string Ping()
        {
            // TODO
            // http://stackoverflow.com/questions/12813151/how-can-i-discover-if-a-device-is-connected-to-a-specific-serial-com-port
            // http://stackoverflow.com/questions/18145475/serial-port-ping-functionality
            
            if (IsOpen)
            {
                var sb = new StringBuilder();
                var response = Transaction(new MessageProperties(), PingMessage);
                sb.Append(string.Format("Data Set Ready: {0} | ", _port.DsrHolding ? "OK" : "-"));
                sb.Append(string.Format("Clear-to-Send: {0} | ", _port.CtsHolding ? "OK" : "-"));
                sb.Append(string.Format("Ping response: {0}", response));

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
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

            // Terminator is added by SerialPort
            #region Append terminator

            sb.Append(properties.TerminalString);

            #endregion Append terminator

            return sb.ToString();
        }

        #endregion Helpers

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
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