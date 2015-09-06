using RS485.UI.Helpers;

namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        public RelayCommand TestBindCommand { get; private set; }

        private void InitializeCommands()
        {
            TestBindCommand = new RelayCommand(ExecuteAction);
        }
    }
}