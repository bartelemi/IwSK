using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
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
        private const string PingMessage = "ping";

        /// <summary>
        /// Message send back when received ping request
        /// </summary>
        private const string PingResponse = "OK";

        private bool _isDTRActive;
        private bool _isDSRActive;
        private bool _isRTSActive;
        private bool _isCTSActive;
        private string _receivedData;
        private Handshake _handshake;
        private MessageType _messageType;
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
        /// Holds the state of Data Set Ready pin
        /// </summary>
        public bool IsDSRActive
        {
            get { return _isDSRActive; }
            set
            {
                if (_isDSRActive != value)
                {
                    _isDSRActive = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Holds the state of Data Terminal Ready pin
        /// </summary>
        public bool IsDTRActive
        {
            get { return _isDTRActive; }
            set
            {
                if (_isDTRActive != value)
                {
                    _isDTRActive = value;
                    _port.DtrEnable = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Holds the state of Clear To Send pin
        /// </summary>
        public bool IsCTSActive
        {
            get { return _isCTSActive; }
            set
            {
                if (_isCTSActive != value)
                {
                    _isCTSActive = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Holds the state of Ready To Send pin
        /// </summary>
        public bool IsRTSActive
        {
            get { return _isRTSActive; }
            set
            {
                if (_isRTSActive != value)
                {
                    _isRTSActive = value;
                    _port.RtsEnable = value;
                    RaisePropertyChanged();
                }
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Current handshake used by port
        /// </summary>
        public Handshake Handshake
        {
            get { return _handshake; }
            set
            {
                _handshake = value;
                _port.Handshake = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Holds type of last received but not processed message
        /// </summary>
        public MessageType MessageType
        {
            get { return _messageType; }
            set
            {
                if (_messageType != value)
                {
                    _messageType = value;
                    RaisePropertyChanged();
                }
            }
        }
        

        #endregion Properties

        #region Initialization

        /// <summary>
        /// Initializes instance of SerialPortHandler
        /// </summary>
        public SerialPortHandler()
        {
            MessageType = MessageType.None;
            InitPinChangedEventHandler();
            InitDataReceivedEventHandler();
        }

        /// <summary>
        /// Initializes handler for pin changed event
        /// </summary>
        private void InitPinChangedEventHandler()
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

            // Initialize timer to check DTR and RTS pins each 100ms
            new Timer(handshake =>
            {
                if ((Handshake)handshake != Handshake.RequestToSendXOnXOff)
                {
                    IsDTRActive = _port.DtrEnable;
                    IsRTSActive = _port.RtsEnable;
                }
            }, Handshake, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(100));
        }

        /// <summary>
        /// Initializes handler for data received event
        /// </summary>
        private void InitDataReceivedEventHandler()
        {
            _port.DataReceived += (o, e) =>
            {
                var text = _port.ReadExisting();

                if (!string.IsNullOrEmpty(text))
                {
                    // Check if it is ping request
                    if (text.Equals(PingMessage))
                    {
                        MessageType = MessageType.PingRequest;
                        SendMessageAsync(new MessageProperties(), PingResponse);
                    }
                    else if (text.Equals(PingResponse))
                    {
                        MessageType = MessageType.PingResponse;
                    }
                    else
                    {
                        MessageType = MessageType.Plain;
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
        public async Task OpenConnectionAsync(string portName, ConnectionSettings connectionSettings)
        {
            try
            {
                if (IsOpen)
                    throw new UnauthorizedAccessException("Port is already opened.");


                if (connectionSettings != null)
                {
                    Handshake = (Handshake)connectionSettings.FlowControl;
                    _port.PortName = portName;
                    _port.BaudRate = (int)(connectionSettings.BitRate);
                    _port.DataBits = connectionSettings.CharacterFormat.DataFieldSize;
                    _port.Parity = (Parity)connectionSettings.CharacterFormat.ParityControl;
                    _port.StopBits = (StopBits)connectionSettings.CharacterFormat.StopBitsNumber;
                    _port.NewLine = connectionSettings.TerminalString;
                    _port.ReadTimeout = connectionSettings.ReadTimeout;
                    _port.WriteTimeout = connectionSettings.WriteTimeout;
                    _port.Encoding = Encoding.ASCII;
                }
                _port.PortName = portName;
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
        /// <returns>Filled structure describing serial port properties</returns>
        public async Task<ConnectionSettings> AutobaudAsync(string portName)
        {
            bool wasOpened = IsOpen;
            var settings = new ConnectionSettings();

            if (!wasOpened)
                OpenConnectionAsync(portName, null);

            // Test all connection settings

            if (!wasOpened)
                CloseConnectionAsync();

            return settings;
        }

        #endregion Estabilishing connection

        #region Data exchange

        /// <summary>
        /// Sends message through serial port
        /// </summary>
        /// <param name="messageProperties">Parameters of message</param>
        /// <param name="message">Message plain text</param>
        public async Task SendMessageAsync(MessageProperties messageProperties, string message)
        {
            try
            {
                var preparedMessage = CreateMessage(message, messageProperties);

                _port.DiscardOutBuffer();
                _port.DiscardInBuffer();
                _port.Write(preparedMessage);
            }
            catch (TimeoutException tmoutEx)
            {
                Debug.WriteLine(tmoutEx.Message);
                throw new TimeoutException("Przekroczono czas połączenia!");
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
        public async Task<string> TransactionAsync(MessageProperties messageProperties, string message)
        {
            SendMessageAsync(messageProperties, message);

            // TODO: wait for response
            //       return received value

            return string.Empty;
        }

        #endregion Data exchange

        #region Testing connection

        /// <summary>
        /// Tests connection and measures Round Trip Delay (RTD) time
        /// </summary>
        public async Task<string> PingAsync()
        {
            if (IsOpen)
            {
                string response;
                var sb = new StringBuilder();
                var stopwatch = new Stopwatch();
                try
                {
                    stopwatch.Start();
                    response = await TransactionAsync(new MessageProperties(), PingMessage);
                }
                catch
                {
                    response = "NONE";
                }
                finally
                {
                    stopwatch.Stop();
                }

                sb.Append(string.Format("Data Set Ready: {0} | ", _port.DsrHolding ? "OK" : "-"));
                sb.Append(string.Format("Clear-to-Send: {0} | ", _port.CtsHolding ? "OK" : "-"));
                sb.Append(string.Format("RTD: {0} ms | ", stopwatch.ElapsedMilliseconds));
                sb.Append(string.Format("Response: {0}{1}", response, Environment.NewLine));

                return sb.ToString();
            }
            else
            {
                return "Port zamknięty!";
            }
        }

        #endregion Testing connection

        #region Closing connection

        /// <summary>
        /// Closes connection
        /// </summary>
        public async Task CloseConnectionAsync()
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

            sb.Append(properties.TerminalString);

            #endregion Append terminator

            return sb.ToString();
        }

        #endregion Helpers

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string name = null)
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