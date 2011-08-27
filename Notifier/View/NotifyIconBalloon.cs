using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Notifier.Model;
using Notifier.Model.Monitor;

namespace Notifier.View
{
    public class NotifyIconBalloon : INotificationView
    {
        private NotifyIcon mNotifyIcon;
        private INotificationModel mModel;

        public INotificationModel Model
        {
            get { return mModel; }
            set
            {
                if (mModel != null)
                    mModel.ContentsUpdated -= OnModelContentsUpdated;

                if (value != null)
                    value.ContentsUpdated += OnModelContentsUpdated;

                mModel = value;
            }
        }

        public NotifyIcon NotifyIcon
        {
            get { return mNotifyIcon; }
            set { mNotifyIcon = value; }
        }

        public int Timeout
        {
            get;
            set;
        }

        protected void OnModelContentsUpdated(object sender, EventArgs e)
        {
            if (mNotifyIcon != null)
            {
                string text = string.Empty;

                int i = 0;

                foreach (IMonitor monitor in Model.Items)
                {
                    if (monitor.ShouldNotify)
                    {
                        text = string.Format(@"{0}{1}{2}", text, string.IsNullOrEmpty(text) ? string.Empty : "\n", monitor.Caption);
                        i++;
                    }
                }

                mNotifyIcon.Text = string.Format(@"{0} task{1} monitoring", Model.Items.Count, Model.Items.Count > 1 ? "s are" : " is");

                string title = string.Format(@"{0} task{1} waiting to be done", i, i > 1 ? "s are" : " is");

                if (!string.IsNullOrEmpty(text))
                    mNotifyIcon.ShowBalloonTip(Timeout, title, text, ToolTipIcon.Info);
            }
        }
    }
}
