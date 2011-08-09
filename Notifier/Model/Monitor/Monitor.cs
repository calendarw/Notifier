using System;
using System.Collections.Generic;
using System.Text;

namespace Notifier.Model.Monitor
{
    public abstract class Monitor : IMonitor
    {
        public abstract string Caption { get; }

        public bool Enabled { get; set; }

        /// <summary>
        /// Format string for format value to caption
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Frequent Type of the monitor
        /// </summary>
        public FrequentType FrequentType { get; set; }

        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Last check time
        /// </summary>
        public DateTime LastCheckTime { get; private set; }

        /// <summary>
        /// Next check time
        /// </summary>
        public DateTime NextCheckTime { get; protected set; }

        public abstract bool ShouldNotify { get; }

        public event EventHandler CheckCompleted;

        public event UnhandledExceptionEventHandler ExceptionThrown;

        public void Check()
        {

            IsCompleted = false;

            try
            {
                PerformCheck();
            }
            catch (Exception ex)
            {
                if (ExceptionThrown != null)
                    ExceptionThrown(this, new UnhandledExceptionEventArgs(ex, false));
            }
            finally
            {
                LastCheckTime = DateTime.Now;
                IsCompleted = true;
            }

            if (CheckCompleted != null)
                CheckCompleted(this, EventArgs.Empty);
        }

        protected abstract void PerformCheck();
    }
}