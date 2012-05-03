using Notifier.Model;
using NUnit.Framework;
using NotifierTest.Mock;

namespace NotifierTest.Model
{
    [TestFixture]
    public class NotificationModelTest
    {
        private INotificationModel mModel;
        private bool contentUpdatedEventFired;

        [SetUp]
        public void SetUp()
        {
            mModel = new NotificationModel();
            mModel.ContentsUpdated += OnContentsUpdated;
        }

        [TearDown]
        public void TearDown()
        {
            mModel.ContentsUpdated -= OnContentsUpdated;
        }

        [Test]
        public void ShouldFireEventWhenExceptionThrown()
        {
            contentUpdatedEventFired = false;
            MockMonitor monitor = new MockMonitor("ExceptionMonitor", true);
            monitor.ShouldThrownException = true;
            mModel.Add(monitor);
            Assert.IsTrue(monitor.ExceptionThrownEventAdded);
            Assert.IsFalse(monitor.ExceptionEventFired);
            mModel.Update();
            System.Threading.Thread.Sleep(1000);    // Multi-thread process time
            Assert.IsTrue(monitor.ExceptionEventFired);
            Assert.IsTrue(contentUpdatedEventFired);
        }

        [Test]
        public void ShouldNotCallCheckIfMonitorIsNotEnabled()
        {
            contentUpdatedEventFired = false;
            MockMonitor monitor = new MockMonitor("DisabledMonitor", true);
            monitor.Enabled = false;
            mModel.Add(monitor);
            Assert.IsFalse(monitor.IsCheckMethodCalled);
            mModel.Update();
            System.Threading.Thread.Sleep(1000);    // Multi-thread process time
            Assert.IsFalse(monitor.IsCheckMethodCalled);
            Assert.IsFalse(contentUpdatedEventFired);
        }

        [Test]
        public void ShouldContentUpdateEventFired()
        {
            contentUpdatedEventFired = false;
            
            MockMonitor monitor1 = new MockMonitor("DisabledMonitor", true);
            monitor1.Enabled = false;
            mModel.Add(monitor1);

            MockMonitor monitor2 = new MockMonitor("EnabledMonitor", true);
            monitor2.Enabled = true;
            mModel.Add(monitor2);

            mModel.Update();
            System.Threading.Thread.Sleep(1000);    // Multi-thread process time
            Assert.IsTrue(contentUpdatedEventFired);
        }

        private void OnContentsUpdated(object sender, System.EventArgs e)
        {
            contentUpdatedEventFired = true;
        }
    }
}
