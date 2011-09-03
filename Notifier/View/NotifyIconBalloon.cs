using System;
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

                UpdateNotifyIconText(); 
            }
        }

        public NotifyIcon NotifyIcon
        {
            get { return mNotifyIcon; }
            set
            {
                if (mNotifyIcon != null)
                    mNotifyIcon.Click -= OnNotifyIconClick;

                if (value != null)
                    value.Click += OnNotifyIconClick;

                mNotifyIcon = value;

                UpdateNotifyIconText(); 
            }
        }

        public int Timeout
        {
            get;
            set;
        }

        protected void OnModelContentsUpdated(object sender, EventArgs e)
        {
            ShowMessage(false);
        }

        private void OnNotifyIconClick(object sender, EventArgs e)
        {
            ShowMessage(true);
        }

        private void ShowMessage(bool showEmpty)
        {
            if (mModel == null || mNotifyIcon == null) return;

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

            if (showEmpty && i == 0)
                text = "Currently no outstanding";
            
            string title = string.Format(@"{0} task{1} waiting to be done", i, string.Format("{0} {1}", i.s(), i.Is()));

            if (!string.IsNullOrEmpty(text))
                mNotifyIcon.ShowBalloonTip(Timeout, title, text, ToolTipIcon.Info);
        }

        private void UpdateNotifyIconText()
        {
            if (mModel == null || mNotifyIcon == null) return;

            mNotifyIcon.Text = string.Format(@"{0} task{1} monitoring", Model.Items.Count, string.Format("{0} {1}", Model.Items.Count.s(), Model.Items.Count.Is()));
        }
    }
}
