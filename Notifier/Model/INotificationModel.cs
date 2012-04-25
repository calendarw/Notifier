using System;
using System.Collections.Generic;
using System.Text;
using Notifier.Model.Monitor;

namespace Notifier.Model
{
    public interface INotificationModel : IModel
    {
        IList<IMonitor> Items { get; }

        void Add(IMonitor item);
        void Clear();
        void Remove(IMonitor item);
        void Update(); 
        
        event UnhandledExceptionEventHandler ExceptionThrown;
    }
}
