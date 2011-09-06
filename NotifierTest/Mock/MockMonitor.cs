using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notifier.Model.Monitor;

namespace NotifierTest.Mock
{
    class MockMonitor : IMonitor
    {
        private string mName;
        private bool mShouldNotify;

        public MockMonitor(string name, bool shouldNotify)
        {
            mName = name;
            mShouldNotify = shouldNotify;
        }

        public string Caption
        {
            get { return mName; }
        }

        public bool Enabled
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        public bool ShouldNotify
        {
            get { return mShouldNotify; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        public void Check()
        {
            throw new NotImplementedException();
        }

        public event EventHandler CheckCompleted;

        public event UnhandledExceptionEventHandler ExceptionThrown;
    }
}
