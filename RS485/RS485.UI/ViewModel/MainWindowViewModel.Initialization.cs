namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        private void InitializeMasterEvents()
        {
            if (ModbusMaster != null)
            {
                ModbusMaster.LogMessageOccured += LogMessageReceived;
                // TODO - other events
            }
        }

        private void InitializeSlaveEvents()
        {
            if (ModbusMaster != null)
            {
                ModbusMaster.LogMessageOccured += LogMessageReceived;
                // TODO - other events
            }
        }

        private void DisposeMaster()
        {
            if (ModbusMaster != null)
            {
                ModbusMaster.LogMessageOccured -= LogMessageReceived;
                // TODO - other events

                ModbusMaster.Dispose();
            }
        }

        private void DisposeSlave()
        {
            if (ModbusSlave != null)
            {
                ModbusSlave.LogMessageOccured -= LogMessageReceived;
                // TODO - other events

                ModbusSlave.Dispose();
            }
        }
    }
}