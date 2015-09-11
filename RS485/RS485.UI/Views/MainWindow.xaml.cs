using System.Windows;
using System.Windows.Controls;
using RS485.UI.ViewModel;
using RS485.Common.Extensions;
using RS485.Common.Model;
using System.Linq;
using System.Windows.Input;
using System.Text;
using RS485.Common.Converters;
using RS485.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace RS485.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
        }

        private void OnErrorTextBoxSlaveTextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorTextBoxSlave.CaretIndex = ErrorTextBoxSlave.Text.Length;
            ErrorTextBoxSlave.ScrollToEnd();
        }

        private void OnErrorTextBoxMasterTextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorTextBox.CaretIndex = ErrorTextBox.Text.Length;
            ErrorTextBox.ScrollToEnd();
        }

        private void OnOutputSlaveTextChanged(object sender, TextChangedEventArgs e)
        {
            OutputSlave.CaretIndex = OutputSlave.Text.Length;
            OutputSlave.ScrollToEnd();
        }

        private void OnOnTransmissionInfoTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TransmissionInfoTextBox.CaretIndex = TransmissionInfoTextBox.Text.Length;
            TransmissionInfoTextBox.ScrollToEnd();
        }

        private void OnOutputMasterTextChanged(object sender, TextChangedEventArgs e)
        {
            OutputMaster.CaretIndex = OutputMaster.Text.Length;
            OutputMaster.ScrollToEnd();
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
