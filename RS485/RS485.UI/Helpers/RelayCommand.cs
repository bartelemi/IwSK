using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace RS485.UI.Helpers
{
    /// <summary>
    /// Class RelayCommand. This class cannot be inherited.
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">canExecute</exception>
        public RelayCommand(Action execute, Func<bool> canExecute)
            : this(execute) 
        {
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _canExecute = canExecute;
        }

        #region ICommand members
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute();
        }
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion
    }

    /// <summary>
    /// Class RelayCommand. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">A generic type, on which will be converted to a command parameter</typeparam>
    public sealed class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<bool> _canExecuteParameterless;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <exception cref="System.ArgumentNullException">execute</exception>
        public RelayCommand(Action<T> execute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">canExecute</exception>
        public RelayCommand(Action<T> execute, Func<bool> canExecute)
            : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _canExecuteParameterless = canExecute;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <exception cref="System.ArgumentNullException">canExecute</exception>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute)
            : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _canExecute = canExecute;
        }

        /// <summary>
        /// Converts the specified parameter to generic object T.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>T.</returns>
        /// <exception cref="System.InvalidCastException">Conversion failed. For more details see inner exception</exception>
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private T Convert(object parameter)
        {
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            if (parameter is T)
                return (T)parameter;
            try
            {
                return (T)System.Convert.ChangeType(parameter, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException("Conversion failed. For more details see inner exception", ex);
            }
        }

        #region ICommand members
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            _execute(Convert(parameter));
        }
        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (_canExecuteParameterless != null)
                return _canExecuteParameterless();
            if (_canExecute != null)
                return _canExecute(Convert(parameter));
            return true;
        }
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion
    }
}
