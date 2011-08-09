using System;
using Notifier.Model.Monitor;
using NUnit.Framework;

namespace NotifierTest.Model.Monitor
{
    public class MonitorTest
    {
        private bool _isEventFired = false;

        public void CheckNormalTest(IMonitor monitor)
        {
            _isEventFired = false;
            monitor.CheckCompleted += monitor_CheckCompleted;
            monitor.ExceptionThrown += monitor_ExceptionThrown;
            monitor.Check();
            Assert.IsTrue(monitor.IsCompleted);
            Assert.IsTrue(_isEventFired);
            monitor.CheckCompleted -= monitor_CheckCompleted;
        }

        private void monitor_ExceptionThrown(object sender, UnhandledExceptionEventArgs e)
        {
            Assert.Fail(e.ExceptionObject.ToString());
        }

        private void monitor_CheckCompleted(object sender, EventArgs e)
        {
            _isEventFired = true;
        }
    }
}
