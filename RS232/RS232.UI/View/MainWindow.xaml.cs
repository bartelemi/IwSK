using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RS232.UI.ViewModel;

namespace RS232.UI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes instance of MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// Checks if input characters are all numbers
        /// </summary>
        /// <param name="sender">Textbox that raised event</param>
        /// <param name="e">Event parameters</param>
        private void Timeout_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(Char.IsNumber);
        }
    }
}