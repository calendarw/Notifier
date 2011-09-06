using System;
using Notifier.Model.Monitor;
using NUnit.Framework;

namespace NotifierTest.Model.Monitor
{
    public abstract class MonitorTest<T>
        where T : IMonitor
    {
        protected Exception ExceptionThrown { get; private set; }
        protected bool IsCompleteEventFired { get; private set; }
        protected bool IsExceptionEventFired { get; private set; }

        protected abstract T GetNormalMonitor();
        protected abstract T GetExceptionMonitor();

        protected void ExceptionCheckCompleted(object sender, EventArgs e)
        {
            IsCompleteEventFired = true;
        }

        protected void ExpectedCheckCompleted(object sender, EventArgs e)
        {
            IsCompleteEventFired = true;
        }

        protected void ExceptionShouldNotThrown(object sender, UnhandledExceptionEventArgs e)
        {
            Assert.Fail(e.ExceptionObject.ToString());
        }

        protected void ExceptionShouldThrown(object sender, UnhandledExceptionEventArgs e)
        {
            IsExceptionEventFired = true;
            ExceptionThrown = (Exception)e.ExceptionObject;
        }

        [SetUp]
        public virtual void Setup()
        {
            ExceptionThrown = null;
            IsCompleteEventFired = false;
            IsExceptionEventFired = false;
        }

        [Test]
        public void ShouldNormal()
        {
            IMonitor monitor = GetNormalMonitor();
            monitor.CheckCompleted += ExpectedCheckCompleted;
            monitor.ExceptionThrown += ExceptionShouldNotThrown;
            monitor.Check();
            Assert.IsTrue(monitor.IsCompleted);
            Assert.IsTrue(IsCompleteEventFired);
            Assert.IsFalse(IsExceptionEventFired);
        }

        [Test]
        public void ShouldThrowException()
        {
            IMonitor monitor = GetExceptionMonitor();
            monitor.CheckCompleted += ExceptionCheckCompleted;
            monitor.ExceptionThrown += ExceptionShouldThrown;
            monitor.Check();
            Assert.IsTrue(IsCompleteEventFired);
            Assert.IsTrue(IsExceptionEventFired);
        }
    }
}
