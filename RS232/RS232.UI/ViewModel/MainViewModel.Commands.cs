using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RS232.Serial;
using RS232.Serial.Model;

namespace RS232.UI.ViewModel
{
    public partial class MainViewModel
    {
        #region Commands

        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand DisconnectCommand { get; private set; }
        public RelayCommand AutobaudCommand { get; private set; }
        public RelayCommand SendCommand { get; private set; }
        public RelayCommand TransactionCommand { get; private set; }
        public RelayCommand PingCommand { get; private set; }
        public RelayCommand InputTypeCommand { get; private set; }
        public RelayCommand ClearMessageTextCommand { get; private set; }

        #endregion Commands

        #region Initialize commands
        private void InitCommands()
        {
            InitConnectCommand();
            InitDisonnectCommand();
            InitAutobaudCommand();
            InitSendCommand();
            InitTransactionCommand();
            InitPingCommand();
            InitInputTypeCommand();
            InitClearMessageTextCommand();
        }

        private void InitConnectCommand()
        {
            ConnectCommand = new RelayCommand(async () =>
            {
                try
                {
                    var settings = new ConnectionSettings
                    {
                        BitRate = BitRate,
                        FlowControl = FlowControl,
                        ReadTimeout = ReadTimeout,
                        WriteTimeout = WriteTimeout,
                        CharacterFormat = CharacterFormat,
                        TerminalString = TerminalSequence.TerminalString
                    };

                    ConnectionState = ConnectionState.Connecting;
                    await _serialPortHandler.OpenConnectionAsync(SelectedPortName, settings);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitDisonnectCommand()
        {
            DisconnectCommand = new RelayCommand(async () =>
            {
                try
                {
                    ConnectionState = ConnectionState.Disconnecting;
                    await _serialPortHandler.CloseConnectionAsync();
                    ConnectionState = ConnectionState.Disconnected;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitAutobaudCommand()
        {
            AutobaudCommand = new RelayCommand(async () =>
            {
                try
                {
                    var lastConnectionState = ConnectionState;
                    ConnectionState = ConnectionState.Autobauding;
                    await _serialPortHandler.AutobaudAsync(SelectedPortName);
                    ConnectionState = lastConnectionState;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;   
                }
            });   
        }

        private void InitSendCommand()
        {
            SendCommand = new RelayCommand(async () =>
            {
                try
                {
                    var properties = new MessageProperties
                    {
                        MessageType = MessageType.Plain,
                        AppendDateTime = AppendDateTime,
                        TerminalString = TerminalSequence.TerminalString,
                    };
                    ConnectionState = ConnectionState.Sending;
                    await _serialPortHandler.SendMessageAsync(properties, MessageText);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
                finally
                {
                    MessageText = string.Empty;
                }
            });
        }

        private void InitTransactionCommand()
        {
            TransactionCommand = new RelayCommand(async () =>
            {
                try
                {
                    var properties = new MessageProperties
                    {
                        MessageType = MessageType.TransactionBegin,
                        AppendDateTime = AppendDateTime,
                        TerminalString = TerminalSequence.TerminalString,
                    };
                    ConnectionState = ConnectionState.Sending;
                    var response = await _serialPortHandler.TransactionAsync(properties, MessageText);
                    ReceivedMessages = response;
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
                finally
                {
                    MessageText = string.Empty;
                }
            });
        }

        private void InitPingCommand()
        {
            PingCommand = new RelayCommand(async () =>
            {
                try
                {
                    var pingMessage = await _serialPortHandler.PingAsync();
                    ReceivedMessages = pingMessage;
                }
                catch (Exception ex)
                {
                    StatusText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitInputTypeCommand()
        {
            InputTypeCommand = new RelayCommand(() =>
            {
                switch (InputType)
                {
                    case InputType.Binary:
                        InputType = InputType.Text;
                        break;
                    case InputType.Text:
                        InputType = InputType.Binary;
                        break;
                }
            });
        }

        private void InitClearMessageTextCommand()
        {
            ClearMessageTextCommand = new RelayCommand(() =>
            {
                MessageText = string.Empty;
            });
        }

        #endregion Initialize commands
    }
}