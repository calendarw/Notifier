using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notifier.Model;
using Notifier.Model.Monitor;
using NUnit.Framework;

namespace NotifierTest.Model.Monitor
{
    public class OverLimitMonitorTest : MonitorTest
    {
        public void LimitTest(OverLimitMonitor monitor, int expectedLimit)
        {
            CheckNormalTest(monitor);
            Assert.AreEqual(expectedLimit, monitor.Current);

            monitor.Limit = expectedLimit;
            Assert.IsFalse(monitor.ShouldNotify);

            monitor.Limit = expectedLimit - 1;
            Assert.IsTrue(monitor.ShouldNotify);
        }
    }
}
