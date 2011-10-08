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

        public MockMonitor(string name, bool shouldNotify, bool shouldThrownException)
            :this(name, shouldNotify)
        {
            ShouldThrownException = shouldThrownException;
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
            private set;
        }

        public bool IsCompleted
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

            IsCompleted = true;

            if (CheckCompleted != null)
                CheckCompleted(this, EventArgs.Empty);
        }

        public event EventHandler CheckCompleted;

        public event UnhandledExceptionEventHandler ExceptionThrown;
    }
}
