using System.Windows;
using Moq;
using RS485.Common.GuiCommon.Core;
using RS485.UI.ViewModel;
using RS485.UI.Views;

namespace RS485.UI
{
    public partial class App : Application
    {
        private readonly IModbusCore _modbusCore;
        private readonly MainWindowViewModel _mainWindowviewModel;
        private MainWindow _mainWindow;

        public App()
        {
            // TODO: Replace mock with real implementation
            var modbusMock = new Mock<IModbusCore>();
            _modbusCore = modbusMock.Object;

            _mainWindowviewModel = new MainWindowViewModel(_modbusCore);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = new MainWindow(_mainWindowviewModel);
            _mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
