namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        private void InitializeMasterEvents()
        {
            if (ModbusMaster != null)
            {
                ModbusMaster.LogMessageOccured += LogMessageReceived;
                ModbusMaster.FirstCommandCompleted += FirstCommandCompletedMaster;
                ModbusMaster.SecondCommandCompleted += SecondCommandCompletedMaster;
                
            }
        }

        private void InitializeSlaveEvents()
        {
            if (ModbusSlave != null)
            {
                ModbusSlave.LogMessageOccured += LogMessageReceived;
                ModbusSlave.FirstCommandReceived += FirstCommandReceivedSlave;
                ModbusSlave.SecondCommandResponseSent += SecondCommandCompletedSlave;
            }
        }

        private void DisposeMaster()
        {
            if (ModbusMaster != null)
            {
                ModbusMaster.LogMessageOccured -= LogMessageReceived;
                ModbusMaster.FirstCommandCompleted -= FirstCommandCompletedMaster;
                ModbusMaster.SecondCommandCompleted -= SecondCommandCompletedMaster;

                ModbusMaster.Dispose();
            }
        }

        private void DisposeSlave()
        {
            if (ModbusSlave != null)
            {
                ModbusSlave.LogMessageOccured -= LogMessageReceived;
                ModbusSlave.FirstCommandReceived -= FirstCommandReceivedSlave;
                ModbusSlave.SecondCommandResponseSent -= SecondCommandCompletedSlave;

                ModbusSlave.Dispose();
            }
        }
    }
}