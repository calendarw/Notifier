using System;
using System.Data;
using Notifier.Model.Monitor;
using NUnit.Framework;
using NUnit.Mocks;

namespace NotifierTest.Model.Monitor
{
    [TestFixture]
    public class RecordCountMonitorTest : OverLimitMonitorTest<RecordCountMonitor>
    {
        private DynamicMock mConnectionMock;
        private DynamicMock mCommandMock;

        protected override RecordCountMonitor GetExceptionMonitor()
        {
            return new RecordCountMonitor();
        }

        protected override RecordCountMonitor GetNormalMonitor()
        {
            mConnectionMock.ExpectAndReturn("get_State", ConnectionState.Open);
            mConnectionMock.ExpectAndReturn("CreateCommand", mCommandMock.MockInstance as IDbCommand);
            string commandText = "SELECT * FROM TEST";
            mCommandMock.Expect("set_CommandText", commandText);

            RecordCountMonitor monitor = new RecordCountMonitor();
            monitor.DbConnection = mConnectionMock.MockInstance as IDbConnection;
            monitor.CommandText = commandText;
            return monitor;
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            mConnectionMock = new DynamicMock(typeof(IDbConnection));
            mCommandMock = new DynamicMock(typeof(IDbCommand));
        }

        [Test]
        [ExpectedException(typeof(OverflowException))]
        public void RecordCountReturnDoubleShouldThrownExceptionTest()
        {
            RecordCountMonitor monitor = GetNormalMonitor();
            mCommandMock.ExpectAndReturn("ExecuteScalar", 9999999999.99999D);
            monitor.ExceptionThrown += ExceptionShouldThrown;
            monitor.Check();
            mCommandMock.Verify();
            monitor.ExceptionThrown -= ExceptionShouldThrown;
            Assert.IsTrue(IsExceptionEventFired);
            Assert.IsNotNull(ExceptionThrown);
            throw ExceptionThrown;
        }

        [Test]
        public void RecordCountReturnNullShouldReturnZeroTest()
        {
            RecordCountMonitor monitor = GetNormalMonitor();
            mCommandMock.ExpectAndReturn("ExecuteScalar", null);
            monitor.Check();
            mCommandMock.Verify();
            Assert.AreEqual(0, monitor.Current);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void RecordCountReturnStringShouldThrownExceptionTest()
        {
            RecordCountMonitor monitor = GetNormalMonitor();
            mCommandMock.ExpectAndReturn("ExecuteScalar", "Text");
            monitor.ExceptionThrown += ExceptionShouldThrown;
            monitor.Check();
            mCommandMock.Verify();
            monitor.ExceptionThrown -= ExceptionShouldThrown;
            Assert.IsTrue(IsExceptionEventFired);
            Assert.IsNotNull(ExceptionThrown);
            throw ExceptionThrown;
        }
    }
}
