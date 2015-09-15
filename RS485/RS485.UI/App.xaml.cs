using System.Windows;
using RS485.UI.ViewModel;
using RS485.UI.Views;

namespace RS485.UI
{
    public partial class App : Application
    {
        private readonly MainWindowViewModel _mainWindowviewModel;
        private MainWindow _mainWindow;

        public App()
        {
            _mainWindowviewModel = new MainWindowViewModel();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _mainWindow = new MainWindow(_mainWindowviewModel);
            _mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
