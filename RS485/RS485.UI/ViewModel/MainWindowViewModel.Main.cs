using RS485.Common.GuiCommon.Core;
using RS485.Common.GuiCommon.Models;
using RS485.Common.GuiCommon.Models.EventArgs;
using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IModbusCore ModbusCore { get { return _modbusCore; } }
        private readonly IModbusCore _modbusCore;

        public MainWindowViewModel(IModbusCore modbusCore)
        {
            _modbusCore = modbusCore;
            IntializeEvents();
            InitializeCommands();
        }

        private void ExecuteAction()
        {
            LogMessageReceived(new LogMessageReceivedEventArgs(LogMessageType.Info, string.Format("Test button: {0}", TestBindProperty)));
        }

        private void LogMessageReceived(LogMessageReceivedEventArgs args)
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