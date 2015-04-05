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
            ConnectCommand = new RelayCommand(() =>
            {
                try
                {
                    var settings = new ConnectionSettings
                    {
                        BitRate = BitRate,
                        FlowControl = FlowControl,
                        PortName = SelectedPortName,
                        CharacterFormat = CharacterFormat,
                        TerminalString = Terminator.ToString()
                    };

                    ConnectionState = ConnectionState.Connecting;
                    _serialPortHandler.OpenConnection(settings);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitDisonnectCommand()
        {
            DisconnectCommand = new RelayCommand(() =>
            {
                try
                {
                    ConnectionState = ConnectionState.Disconnecting;
                    _serialPortHandler.CloseConnection();
                    ConnectionState = ConnectionState.Disconnected;
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitAutobaudCommand()
        {
            AutobaudCommand = new RelayCommand(() =>
            {
                try
                {
                    ConnectionState = ConnectionState.Autobauding;
                    _serialPortHandler.Autobaud(SelectedPortName);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    ConnectionState = ConnectionState.Error;   
                }
            });   
        }

        private void InitSendCommand()
        {
            SendCommand = new RelayCommand(() =>
            {
                try
                {
                    var properties = new MessageProperties
                    {
                        Terminator = Terminator,
                        AppendDateTime = AppendDateTime,
                        CustomTerminator = CustomTerminator
                    };
                    ConnectionState = ConnectionState.Sending;
                    _serialPortHandler.SendMessage(properties, MessageText);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    ConnectionState = ConnectionState.Error;   
                }
            });
        }

        private void InitTransactionCommand()
        {
            TransactionCommand = new RelayCommand(() =>
            {
                try
                {
                    var properties = new MessageProperties
                    {
                        Terminator = Terminator,
                        AppendDateTime = AppendDateTime,
                        CustomTerminator = CustomTerminator
                    };
                    ConnectionState = ConnectionState.Sending;
                    _serialPortHandler.Transaction(properties, MessageText);
                    ConnectionState = ConnectionState.Connected;
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    ConnectionState = ConnectionState.Error;
                }
            });
        }

        private void InitPingCommand()
        {
            PingCommand = new RelayCommand(() =>
            {
                try
                {
                    _serialPortHandler.Ping();
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
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