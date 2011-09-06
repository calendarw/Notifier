using System;
using Notifier.View;
using NotifierTest.Mock;
using NUnit.Framework;
using System.Windows.Forms;

namespace NotifierTest.View
{
    public abstract class NotificationViewTest
    {
        private NotifyIcon mNotifyIcon;
        private INotificationView mView;
        protected abstract INotificationView GetView();

        protected NotificationViewTest()
        {
            mNotifyIcon = new NotifyIcon();
        }

        [SetUp]
        public virtual void Setup()
        {
            mView = GetView();
            mView.Model = new MockModel();
            mView.NotifyIcon = mNotifyIcon;
        }

        [Test]
        public void SingleItemNoOutstandingTest()
        {
            mView.Model.Add(new MockMonitor("No Outstanding", false));
            mView.Model.Update();
        }

        [Test]
        public void SingleItemOutstandingTest()
        {
            mView.Model.Add(new MockMonitor("Outstanding", true));
            mView.Model.Update();
        }
    }
}
