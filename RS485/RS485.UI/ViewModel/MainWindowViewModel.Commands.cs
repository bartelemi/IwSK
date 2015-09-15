using RS485.Common.Model;
using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        public RelayCommand ExecuteModbusRequestCommand { get; private set; }
        public RelayCommand StartSlaveListenerCommand { get; private set; }
        public RelayCommand<MasterSlave> ConnectToModbusCommand { get; private set; }


        private void InitializeCommands()
        {
            ExecuteModbusRequestCommand = new RelayCommand(ExecuteModbusRequest);
            StartSlaveListenerCommand   = new RelayCommand(StartSlaveListener);
            ConnectToModbusCommand             = new RelayCommand<MasterSlave>(ConnectToModbus);
        }
    }
}