using Notifier.Model;
using NUnit.Framework;

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
        public void UnhandledExceptionThrownTest()
        {
            Mock.MockMonitor monitor = new Mock.MockMonitor("ExceptionMonitor", true, true);
            mModel.Add(monitor);
            Assert.IsTrue(monitor.ExceptionThrownEventAdded);
            Assert.IsFalse(monitor.ExceptionEventFired);
            monitor.Check();
            Assert.IsTrue(monitor.ExceptionEventFired);
        }
    }
}
