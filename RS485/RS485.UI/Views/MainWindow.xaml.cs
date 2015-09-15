using System.Windows;
using System.Windows.Controls;
using RS485.UI.ViewModel;
using System.Linq;
using System.Windows.Input;

namespace RS485.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
        }

        private void OutputTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            OutputTextBox.CaretIndex = OutputTextBox.Text.Length;
            OutputTextBox.ScrollToEnd();
        }

        /// <summary>
        /// Checks if input characters are all numbers
        /// </summary>
        /// <param name="sender">Textbox that raised event</param>
        /// <param name="e">Event parameters</param>
        private void Textbox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(System.Char.IsNumber);
        }
    }
}
