using Notifier.Model;
using NUnit.Framework;
using NotifierTest.Mock;

namespace NotifierTest.Model
{
    [TestFixture]
    public class NotificationModelTest
    {
        private INotificationModel mModel;

        [SetUp]
        public void SetUp()
        {
            mModel = new NotificationModel();
        }

        [Test]
        public void ShouldFireEventWhenExceptionThrown()
        {
            MockMonitor monitor = new MockMonitor("ExceptionMonitor", true);
            monitor.ShouldThrownException = true;
            mModel.Add(monitor);
            Assert.IsTrue(monitor.ExceptionThrownEventAdded);
            Assert.IsFalse(monitor.ExceptionEventFired);
            mModel.Update();
            System.Threading.Thread.Sleep(1000);    // Multi-thread process time
            Assert.IsTrue(monitor.ExceptionEventFired);
        }

        [Test]
        public void ShouldNotCallCheckIfMonitorIsNotEnabled()
        {
            MockMonitor monitor = new MockMonitor("DisabledMonitor", true);
            monitor.Enabled = false;
            mModel.Add(monitor);
            Assert.IsFalse(monitor.IsCheckMethodCalled);
            mModel.Update();
            System.Threading.Thread.Sleep(1000);    // Multi-thread process time
            Assert.IsFalse(monitor.IsCheckMethodCalled);
        }
    }
}
