namespace RS485.UI.ViewModel
{
    public partial class MainWindowViewModel
    {
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }
        private bool _isProcessing;

        public object OutputTextBoxContent
        {
            get { return _outputTextBoxContent; }
            set
            {
                _outputTextBoxContent = value; 
                OnPropertyChanged();
            }
        }
        private object _outputTextBoxContent;

        public string TestBindProperty
        {
            get { return _testBindProperty; }
            set
            {
                _testBindProperty = value;
                OnPropertyChanged();
            }
        }
        private string _testBindProperty;
    }
}