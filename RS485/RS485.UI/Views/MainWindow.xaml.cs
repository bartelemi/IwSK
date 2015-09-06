using System.Windows;
using System.Windows.Controls;
using RS485.UI.ViewModel;

namespace RS485.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void OnLogTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            LogTextBox.CaretIndex = LogTextBox.Text.Length;
            LogTextBox.ScrollToEnd();
        }
    }
}
