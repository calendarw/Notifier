using System;
using System.Collections.Generic;
using System.Threading;
using Notifier.Controller;
using Notifier.Model;
using NotifierTest.Mock;
using NUnit.Framework;

namespace NotifierTest.Controller
{
    [TestFixture]
    public class NotificationControllerTest
    {
        NotificationController mController;
        MockModel mModel = new MockModel();
        int sleepDuration = 3500;
        int interval = 1000;

        [SetUp]
        public void Init()
        {

            mController = new NotificationController();
            mController.Interval = interval;
            mController.Model = mModel;
        }

        [Test]
        public void NormalTest()
        {
            mModel.ProcessingTime = 0;
            DateTime checkTime = DateTime.Now;
            Thread.Sleep(interval);
            mController.Start();
            Thread.Sleep(sleepDuration);
            mController.Stop();
            // for check will timer trigger anymore after stop
            Thread.Sleep(interval);
            Assert.AreEqual(sleepDuration / interval, mModel.UpdateTimes.Count);
            mModel.UpdateTimes.ForEach(o => { Assert.IsTrue((o - checkTime).TotalMilliseconds > interval); checkTime = o; });
        }

        /*
        [Test]
        public void SlowTest()
        {
            mModel.ProcessingTime = 800;
            DateTime checkTime = DateTime.Now;
            Thread.Sleep(interval);
            mController.Start();
            Thread.Sleep(sleepDuration);
            mController.Stop();
            // for check will timer trigger anymore after stop
            Thread.Sleep(interval);
            Assert.AreEqual(sleepDuration / interval, mModel.UpdateTimes.Count);
            mModel.UpdateTimes.ForEach(o => { Assert.IsTrue((o - checkTime).TotalMilliseconds > interval); checkTime = o; });
        }44444444444444
         */
    }
}