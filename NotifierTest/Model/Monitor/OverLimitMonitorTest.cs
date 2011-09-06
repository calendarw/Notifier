using Notifier.Model.Monitor;
using NUnit.Framework;

namespace NotifierTest.Model.Monitor
{
    public abstract class OverLimitMonitorTest<T> : MonitorTest<T>
        where T : OverLimitMonitor
    {
        [Test]
        public void BelowLimitTest()
        {
            OverLimitMonitor monitor = GetNormalMonitor();
            monitor.Check();
            int expected = monitor.Current;
            monitor.Limit = expected + 1;
            Assert.IsFalse(monitor.ShouldNotify);
        }

        [Test]
        public void EqualTest()
        {
            OverLimitMonitor monitor = GetNormalMonitor();
            monitor.Check();
            int expected = monitor.Current;
            monitor.Limit = expected;
            Assert.IsFalse(monitor.ShouldNotify);
        }

        [Test]
        public void OverLimitTest()
        {
            OverLimitMonitor monitor = GetNormalMonitor();
            monitor.Check();
            int expected = monitor.Current;
            monitor.Limit = expected - 1;
            Assert.IsTrue(monitor.ShouldNotify);
        }
    }
}
