using RS485.Common.Exceptions;
using RS485.Serial.Model;
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
//using System.Windows.Media.Animation;

namespace RS485.Common.Serial
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
            _port.DataReceived += new SerialDataReceivedEventHandler(SerialPortDataReceived);
            InitPinChangedEventHandler();
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

        #endregion Initialization

        #region Communication

        #region Estabilishing connection

        /// <summary>
        /// Opens new serial port connection
        /// </summary>
        /// <param name="connectionSettings">Serial port connection settings</param>
        public Task OpenConnectionAsync(ConnectionSettings connectionSettings)
        {
            return Task.Run(() =>
            {
                if (connectionSettings != null)
                {
                    if (connectionSettings.FlowControl == FlowControl.None ||
                        connectionSettings.FlowControl == FlowControl.Manual)
                    {
                        Handshake = Handshake.None;
                    }
                    else
                    {
                        Handshake = (Handshake)connectionSettings.FlowControl;
                    }

                    _port.PortName = connectionSettings.PortName;
                    _port.BaudRate = (int)(connectionSettings.BitRate);
                    _port.DataBits = connectionSettings.CharacterFormat.DataFieldSize;
                    _port.Parity = (Parity)connectionSettings.CharacterFormat.ParityControl;
                    _port.StopBits = (StopBits)connectionSettings.CharacterFormat.StopBitsNumber;
                    _port.NewLine = connectionSettings.TerminalString;
                    _port.ReadTimeout = connectionSettings.ReadTimeout;
                    _port.WriteTimeout = connectionSettings.WriteTimeout;
                    _port.Encoding = Encoding.ASCII;
                    _port.PortName = connectionSettings.PortName;
                }
                try
                {
                    if (IsOpen)
                        return;
                    _port.Open();
                }
                catch (Exception e)
                {
                    if (IsOpen)
                        return;
                    else
                        throw new InternalErrorException(e.Message);
                }
            });
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
                await OpenConnectionAsync(null);

            // Test all connection settings

            if (!wasOpened)
                await CloseConnectionAsync();

            return settings;
        }

        #endregion Estabilishing connection

        #region Data exchange

        #region Data receive

        /// <summary>
        /// Data received delegate
        /// </summary>
        /// <param name="data">Data</param>
        public delegate void DataReceived(string data, MessageType type);

        /// <summary>
        /// Raised when data received
        /// </summary>
        public event DataReceived OnDataReceived;

        /// <summary>
        /// Handles serial port data received event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private async void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string text = _port.ReadExisting();
            _incomingMessage.Append(text);

            if (text.EndsWith(_port.NewLine))
            {
                bool isPreprocessed = DismantleMessage(_incomingMessage.ToString());
                _incomingMessage.Clear();

                if (isPreprocessed)
                {
                    if (MessageType == MessageType.PingRequest)
                    {
                        var settings = new MessageProperties
                        {
                            AppendDateTime = false,
                            MessageType = MessageType.PingResponse,
                            TerminalString = _port.NewLine
                        };
                        await SendMessageAsync(settings, PingResponse);
                    }

                    if(OnDataReceived != null)
                        OnDataReceived(ReceivedData, MessageType);
                }
            }
        }

        #endregion Data receive

        #region Data send

        public Task SendMessageAsync(string message)
        {
            var settings = new MessageProperties
                        {
                            AppendDateTime = false,
                            MessageType = MessageType.Plain,
                            TerminalString = _port.NewLine
                        };
            return SendMessageAsync(settings, message);
        }
        /// <summary>
        /// Sends message through serial port
        /// </summary>
        /// <param name="messageProperties">Parameters of message</param>
        /// <param name="message">Message plain text</param>
        public Task SendMessageAsync(MessageProperties messageProperties, string message)
        {
            return Task.Run(() =>
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
                    throw new InternalErrorException("Przekroczono czas połączenia!");
                }
                catch (InvalidOperationException invOpEx)
                {
                    throw new InternalErrorException("Port jest zamknięty!");
                }
                catch (IOException ioEx)
                {
                    throw new InternalErrorException("Ogólny błąd I/O!");
                }
                catch (Exception ex)
                {
                    throw new InternalErrorException("Nieznany błąd!");
                }
            });
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
            await SendMessageAsync(messageProperties, message);

            var t = Task.Run(() =>
            {
                while (MessageType != MessageType.TransactionEnd &&
                       MessageType != MessageType.PingResponse)
                {

                }
            });
            if (t.Wait(_port.ReadTimeout))
            {
                return ReceivedData;
            }
            return string.Empty;
        }

        #endregion Data send

        #endregion Data exchange

        #region Testing connection

        /// <summary>
        /// Tests connection and measures Round Trip Delay (RTD) time
        /// </summary>
        public async Task<string> PingAsync()
        {
            return Task.Run(async () =>
            {
                string response = string.Empty;
                string expectedResponse = string.Format("{0}{1}", PingResponse, _port.NewLine);
                var sb = new StringBuilder();
                var stopwatch = new Stopwatch();
                try
                {
                    var settings = new MessageProperties
                    {
                        MessageType = MessageType.PingRequest,
                        AppendDateTime = false,
                        TerminalString = _port.NewLine
                    };

                    stopwatch.Start();
                    response = await TransactionAsync(settings, PingMessage);
                    if (response != expectedResponse)
                    {
                        response = "Err";
                    }
                }
                catch
                {
                    response = "None";
                }
                finally
                {
                    stopwatch.Stop();
                }

                sb.Append(string.Format("Data Set Ready: {0} | ", _port.DsrHolding ? "OK" : "--"));
                sb.Append(string.Format("Clear-to-Send: {0} | ", _port.CtsHolding ? "OK" : "--"));
                sb.Append(string.Format("RTD: {0} ms | ", stopwatch.ElapsedMilliseconds));
                sb.Append(string.Format("Response: {0}", response));

                return sb.ToString();
            }).Result;
        }

        #endregion Testing connection

        #region Closing connection

        /// <summary>
        /// Closes connection
        /// </summary>
        public Task CloseConnectionAsync()
        {
            return Task.Run(() =>
            {
                if (IsOpen)
                {
                    _port.DiscardOutBuffer();
                    _port.DiscardInBuffer();
                    _port.Close();
                }
            });
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
            int messageType = (int) properties.MessageType;
            var sb = new StringBuilder(messageType.ToString("00"));

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

        /// <summary>
        /// Checks received message and returns it's type according to header
        /// </summary>
        /// <param name="input">Received message</param>
        /// <returns>True if message preprocessed successfully</returns>
        private bool DismantleMessage(string input)
        {
            try
            {
                bool isWellFormated = false;

                // Try to get message type
                int type;
                if (int.TryParse(input.Substring(0, 2), out type))
                {
                    MessageType messageType;
                    if (MessageType.TryParse(type.ToString(), out messageType))
                    {
                        MessageType = messageType;
                        isWellFormated = true;
                    }
                }

                // Try to get message data
                if (isWellFormated)
                {    
                    ReceivedData = input.Substring(2, input.Length - 2);
                }
                else
                {
                    throw new FormatException("Bad format of message.");
                }

                return true;
            }
            catch
            {
                MessageType = MessageType.Error;
                ReceivedData = string.Empty;
                return false;
            }
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