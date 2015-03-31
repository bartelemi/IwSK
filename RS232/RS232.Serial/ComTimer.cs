using System;
using System.Timers;

namespace RS232.Serial
{
    /// <summary>
    /// Timer used for serial port communication
    /// </summary>
    public class ComTimer : IDisposable
    {
        #region Properties and fields
        private Timer timer = new Timer();

        /// <summary>
        /// Tells if there was time for response run out
        /// </summary>
        public bool Timeout { get; set; }

        #endregion Properties and fields

        #region Initialize

        /// <summary>
        /// Initializes instance of ComTimer
        /// </summary>
        public ComTimer() : this(1000)
        {
        }

        /// <summary>
        /// Initializes instance of ComTimer
        /// </summary>
        /// <param name="interval">Interval of timeout in miliseconds</param>
        public ComTimer(double interval)
        {
            Timeout = false;
            timer.Enabled = false;
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(OnTimeoutEventHandler);
        }

        #endregion Initialize

        #region Timer functions

        /// <summary>
        /// Starts timer
        /// </summary>
        public void Start()
        {
            Timeout = false;
            timer.Stop();
            timer.Start();
        }

        /// <summary>
        /// Sets new timeout interval and starts timer
        /// </summary>
        /// <param name="timeout">New timeout interval</param>
        public void Start(double timeout)
        {
            timer.Interval = timeout;
            Start();
        }

        #endregion Timer functions

        #region Events

        /// <summary>
        /// Handler for timer timeout event
        /// </summary>
        /// <param name="source">Source of event</param>
        /// <param name="e">Event arguments</param>
        private void OnTimeoutEventHandler(object source, ElapsedEventArgs e)
        {
            Timeout = true;
            timer.Stop();
        }

        #endregion Events

        #region IDisposable

        /// <summary>
        /// Disposes timer
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }

        #endregion IDisposable
    }
}