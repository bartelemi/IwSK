using System.Windows;
using Moq;
using RS485.Common.Interfaces;
using RS485.UI.ViewModel;
using RS485.UI.Views;

namespace RS485.UI
{
    public partial class App : Application
    {
        private readonly IModbusMaster _modbusMaster;
        private readonly IModbusSlave _modbusSlave;

        private readonly MainWindowViewModel _mainWindowviewModel;
        private MainWindow _mainWindow;

        public App()
        {
            // TODO: Replace mocks with real implementations
            _modbusMaster = new Mock<IModbusMaster>().Object;
            _modbusSlave = new Mock<IModbusSlave>().Object;

            _mainWindowviewModel = new MainWindowViewModel(_modbusMaster, _modbusSlave);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = new MainWindow(_mainWindowviewModel);
            _mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
