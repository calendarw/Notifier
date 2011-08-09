using System;

namespace Notifier.Model.Monitor
{
    public interface IMonitor
    {
        /// <summary>
        /// Caption for display
        /// </summary>
        string Caption { get; }

        /// <summary>
        /// 
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Flag for indicate should notify
        /// </summary>
        bool ShouldNotify { get; }

        /// <summary>
        /// Flag for indicate the check is completed
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Check current status
        /// </summary>
        void Check();
        
        /// <summary>
        /// Occurs when check status completed
        /// </summary>
        event EventHandler CheckCompleted;

        event UnhandledExceptionEventHandler ExceptionThrown;
    }
}