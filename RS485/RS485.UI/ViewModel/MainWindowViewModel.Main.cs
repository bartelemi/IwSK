using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.Common.Interfaces;
using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IModbusMaster ModbusMaster { get { return _modbusMaster; } }
        private readonly IModbusMaster _modbusMaster;

        private IModbusSlave ModbusSlave { get { return _modbusSlave; } }
        private readonly IModbusSlave _modbusSlave;

        public MainWindowViewModel(IModbusMaster modbusMaster, IModbusSlave modbusSlave)
        {
            _modbusMaster = modbusMaster;
            _modbusSlave = modbusSlave;

            IntializeEvents();
            InitializeCommands();
        }

        private void ExecuteAction()
        {
            LogMessageReceived(new LogMessageOccuredEventArgs(LogMessageType.Info, string.Format("Test button: {0}", TestBindProperty)));
        }

        private void LogMessageReceived(LogMessageOccuredEventArgs args)
        {
            OutputTextBoxContent += args.ToReadableLogMessage();
        }

        #region Dispose area

        public override void Dispose()
        {
            DisposeEvents();
            base.Dispose();
        }

        #endregion
    }
}