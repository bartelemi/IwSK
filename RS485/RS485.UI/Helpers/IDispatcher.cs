using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RS485.UI.Helpers
{
    /// <summary>
    /// Interface IDispatcher.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Invokes the specified action synchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        void Invoke(Action action);

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        void Invoke(Action action, DispatcherPriority priority);

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken);

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token or after specified timeout.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="timeout">The timeout.</param>
        void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout);

        /// <summary>
        /// Invokes the specified callback synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns>TResult.</returns>
        TResult Invoke<TResult>(Func<TResult> callback);

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>TResult.</returns>
        TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority);

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>TResult.</returns>
        TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken);

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token or after specified timeout.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>TResult.</returns>
        TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout);

        /// <summary>
        /// Invokes the specified action asynchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Task.</returns>
        Task InvokeAsync(Action action);

        /// <summary>
        /// Invokes the specified action asynchronously with specified dispacher priority.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Task.</returns>
        Task InvokeAsync(Action action, DispatcherPriority priority);

        /// <summary>
        /// Invokes the specified action asynchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        Task InvokeAsync(Action action, DispatcherPriority priority, CancellationToken cancellationToken);

        /// <summary>
        /// Invokes the specified callback asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> callback);

        /// <summary>
        /// Invokes the specified callback asynchronously with specified dispacher priority.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority);

        /// <summary>
        /// Invokes the specified callback asynchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken);
    }

    public class DispatcherWrapper : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherWrapper"/> class.
        /// If Application.Current isn't null (real app, not unit tests) load application dispatcher.
        /// </summary>
        public DispatcherWrapper()
        {
            if (Application.Current != null) _dispatcher = Application.Current.Dispatcher;
        }

        /// <summary>
        /// Invokes the specified action synchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        public void Invoke(Action action, DispatcherPriority priority)
        {
            _dispatcher.Invoke(action, priority);
        }

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            _dispatcher.Invoke(action, priority, cancellationToken);
        }

        /// <summary>
        /// Invokes the specified action synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token or after specified timeout.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="timeout">The timeout.</param>
        public void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            _dispatcher.Invoke(action, priority, cancellationToken, timeout);
        }

        /// <summary>
        /// Invokes the specified callback synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns>TResult.</returns>
        public TResult Invoke<TResult>(Func<TResult> callback)
        {
            return _dispatcher.Invoke(callback);
        }

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>TResult.</returns>
        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority)
        {
            return _dispatcher.Invoke(callback, priority);
        }

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>TResult.</returns>
        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return _dispatcher.Invoke(callback, priority, cancellationToken);
        }

        /// <summary>
        /// Invokes the specified callback synchronously with specified dispacher priority.
        /// Can be canceled using cancelation token or after specified timeout.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>TResult.</returns>
        public TResult Invoke<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            return _dispatcher.Invoke(callback, priority, cancellationToken, timeout);
        }

        /// <summary>
        /// Invokes the specified action asynchronously.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Task.</returns>
        public async Task InvokeAsync(Action action)
        {
            await _dispatcher.InvokeAsync(action);
        }

        /// <summary>
        /// Invokes the specified action asynchronously with specified dispacher priority.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Task.</returns>
        public async Task InvokeAsync(Action action, DispatcherPriority priority)
        {
            await _dispatcher.InvokeAsync(action, priority);
        }

        /// <summary>
        /// Invokes the specified action asynchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public async Task InvokeAsync(Action action, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            await _dispatcher.InvokeAsync(action, priority, cancellationToken);
        }

        /// <summary>
        /// Invokes the specified callback asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public async Task<TResult> InvokeAsync<TResult>(Func<TResult> callback)
        {
            return await _dispatcher.InvokeAsync(callback);
        }

        /// <summary>
        /// Invokes the specified callback asynchronously with specified dispacher priority.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public async Task<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority)
        {
            return await _dispatcher.InvokeAsync(callback, priority);
        }

        /// <summary>
        /// Invokes the specified callback asynchronously with specified dispacher priority.
        /// Can be canceled using cancelation token.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="callback">The callback.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        public async Task<TResult> InvokeAsync<TResult>(Func<TResult> callback, DispatcherPriority priority, CancellationToken cancellationToken)
        {
            return await _dispatcher.InvokeAsync(callback, priority, cancellationToken);
        }
    }
}
