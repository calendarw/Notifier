using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using System.Data;
using Notifier.Model.Monitor;

namespace NotifierTest.Model.Monitor
{
    [TestFixture]
    class RecordCountMonitorTest : OverLimitMonitorTest
    {
        private DynamicMock mConnectionMock;
        private DynamicMock mCommandMock;
        private RecordCountMonitor mMonitor;

        [SetUp]
        public void Init()
        {
            mConnectionMock = new DynamicMock(typeof(IDbConnection));
            mCommandMock = new DynamicMock(typeof(IDbCommand));
            mConnectionMock.ExpectAndReturn("get_State", ConnectionState.Open);
            mConnectionMock.ExpectAndReturn("CreateCommand", mCommandMock.MockInstance as IDbCommand);
            string commandText = "SELECT * FROM TEST";
            mCommandMock.Expect("set_CommandText", commandText);

            mMonitor = new RecordCountMonitor();
            mMonitor.DbConnection = mConnectionMock.MockInstance as IDbConnection;
            mMonitor.CommandText = commandText;
        }

        [Test]
        public void RecordCountReturnZeroTest()
        {
            int expected = 0;
            mCommandMock.ExpectAndReturn("ExecuteScalar", expected);
            LimitTest(mMonitor, expected);
            mCommandMock.Verify();
        }

        [Test]
        public void RecordCountReturnTenTest()
        {
            int expected = 10;
            mCommandMock.ExpectAndReturn("ExecuteScalar", expected);
            LimitTest(mMonitor, expected);
            mCommandMock.Verify();
        }
    }
}
