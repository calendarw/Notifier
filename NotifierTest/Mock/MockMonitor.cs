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
            Enabled = true;
            IsCheckMethodCalled = false;
        }

        public string Caption
        {
            get { return mName; }
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool ExceptionEventFired
        {
            get;
            private set;
        }

        public bool ExceptionThrownEventAdded
        {
            get { return ExceptionThrown != null; }
        }

        public bool ShouldNotify
        {
            get { return mShouldNotify; }
        }

        public bool ShouldThrownException
        {
            get;
            set;
        }

        public bool IsCompleted
        {
            get;
            private set;
        }

        public bool IsCheckMethodCalled
        {
            get;
            private set;
        }

        public void Check()
        {
            IsCompleted = false;

            ExceptionEventFired = false;

            if (ShouldThrownException && ExceptionThrownEventAdded)
            {
                ExceptionThrown(this, new UnhandledExceptionEventArgs(new Exception(), false));
                ExceptionEventFired = true;
            }

            IsCheckMethodCalled = true;

            IsCompleted = true;

            if (CheckCompleted != null)
                CheckCompleted(this, EventArgs.Empty);
        }

        public event EventHandler CheckCompleted;

        public event UnhandledExceptionEventHandler ExceptionThrown;
    }
}
