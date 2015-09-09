namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        private void IntializeEvents()
        {
            ModbusMaster.LogMessageOccured += LogMessageReceived;
            ModbusSlave.LogMessageOccured  += LogMessageReceived;
        }

        private void DisposeEvents()
        {
            ModbusMaster.LogMessageOccured -= LogMessageReceived;
            ModbusSlave.LogMessageOccured  -= LogMessageReceived;
        }
    }
}