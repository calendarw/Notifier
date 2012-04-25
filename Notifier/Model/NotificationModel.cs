using System;
using System.Collections.Generic;
using System.Threading;
using Notifier.Model.Monitor;

namespace Notifier.Model
{
    public class NotificationModel : INotificationModel
    {
        private List<IMonitor> mMonitors = new List<IMonitor>();
        private bool mWorking;

        public IList<IMonitor> Items
        {
            get { return mMonitors.AsReadOnly(); }
        }

        public event EventHandler ContentsUpdated;

        public event UnhandledExceptionEventHandler ExceptionThrown;

        public void Add(IMonitor item)
        {
            if (item == null)
                throw new ArgumentNullException("item should not be null");
            if (mMonitors.Contains(item)) return;
            item.CheckCompleted += item_CheckCompleted;
            item.ExceptionThrown += item_ExceptionThrown;
            mMonitors.Add(item);
        }

        public void Clear()
        {
            mMonitors.ForEach(o =>
            {
                o.CheckCompleted -= item_CheckCompleted;
                o.ExceptionThrown -= item_ExceptionThrown;
            });
            mMonitors.Clear();
        }

        public void Remove(IMonitor item)
        {
            if (item == null)
                throw new ArgumentNullException("item should not be null");
            if (!mMonitors.Contains(item)) return;
            item.CheckCompleted -= item_CheckCompleted;
            item.ExceptionThrown -= item_ExceptionThrown;
            mMonitors.Remove(item);
        }

        public void Update()
        {
            if (mWorking) return;

            mWorking = true;

            mMonitors.FindAll(o => o.Enabled).ForEach(o =>
            {
                Thread t = new Thread(new ThreadStart(o.Check));
                t.Start();
            });
        }

        private void item_CheckCompleted(object sender, EventArgs e)
        {
            mWorking = mMonitors.FindAll(o => !o.IsCompleted).Count > 0;

            if (ContentsUpdated != null)
                ContentsUpdated(sender, e);
        }

        private void item_ExceptionThrown(object sender, UnhandledExceptionEventArgs e)
        {
            if (ExceptionThrown != null)
                ExceptionThrown(sender, e);
        }
    }
}
