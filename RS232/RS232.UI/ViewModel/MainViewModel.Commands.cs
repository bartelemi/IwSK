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

        public RelayCommand SendCommand { get; private set; }
        public RelayCommand TransactionCommand { get; private set; }
        public RelayCommand PingCommand { get; private set; }
        public RelayCommand InputTypeCommand { get; private set; }
        public RelayCommand ClearMessageTextCommand { get; private set; }

        #endregion Commands

        #region Initialize commands
        private void InitCommands()
        {
            InitSendCommand();
            InitTransactionCommand();
            InitPingCommand();
            InitInputTypeCommand();
            InitClearMessageTextCommand();
        }

        private void InitSendCommand()
        {
            SendCommand = new RelayCommand(() =>
            {
                var properties = new MessageProperties
                {
                    Terminator = Terminator,
                    AppendDateTime = AppendDateTime,
                    CustomTerminator = CustomTerminator
                };
                var communicator = new SerialPortHandler();
                communicator.SendMessage(new ConnectionSettings(), new MessageProperties(), MessageText);
            });
        }

        private void InitTransactionCommand()
        {
            TransactionCommand = new RelayCommand(() =>
            {
            });
        }

        private void InitPingCommand()
        {
            PingCommand = new RelayCommand(() =>
            {
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