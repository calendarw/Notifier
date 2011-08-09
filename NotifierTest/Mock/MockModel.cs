using System;
using System.Collections.Generic;
using System.Threading;
using Notifier.Model;
using Notifier.Model.Monitor;

namespace NotifierTest.Mock
{
    class MockModel : INotificationModel
    {
        public MockModel()
        {
            UpdateTimes = new List<DateTime>();
            Items = new List<IMonitor>();
        }

        public IList<IMonitor> Items
        {
            get;
            private set;
        }

        public List<DateTime> UpdateTimes
        {
            get;
            private set;
        }

        public int ProcessingTime { get; set; }

        public event EventHandler ContentsUpdated;

        public void Add(IMonitor item)
        {
            throw new NotImplementedException();
        }

        public void Remove(IMonitor item)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            UpdateTimes.Add(DateTime.Now);
            if (ProcessingTime > 0)
                Thread.Sleep(ProcessingTime);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
