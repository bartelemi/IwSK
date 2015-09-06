namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        private void IntializeEvents()
        {
            ModbusCore.LogMessageReceived += LogMessageReceived;
        }

        private void DisposeEvents()
        {
            ModbusCore.LogMessageReceived -= LogMessageReceived;
        }
    }
}