using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RS485.UI.Helpers
{
    /// <summary>
    /// Class ViewModelBase.
    /// Base class for all classes used to display content using MVVM pattern.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        public IDispatcher Dispatcher
        {
            get { return _dispatcher; }
            set { _dispatcher = value; }
        }
        private IDispatcher _dispatcher = new DispatcherWrapper();

        #region INotifyPropertyChanged members

        /// <summary>
        /// Occurs when MVVM binding property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when MVVM binding property changed.
        /// Can determine property name.
        /// <example>
        /// public string ExampleString
        /// {
        ///     get { return _exampleString; }
        ///     set
        ///     {
        ///         _exampleString = value;
        ///         OnPropertyChanged();
        ///     }
        /// }
        /// private string _exampleString;
        /// </example>
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IDataErorInfo members
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public virtual string Error
        {
            get { return null; }
        }
        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.String.</returns>
        public virtual string this[string propertyName] { get { return string.Empty; } }
        #endregion

        public virtual void Dispose()
        {
        }
    }
}
