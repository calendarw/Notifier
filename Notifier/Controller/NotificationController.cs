using System.Timers;
using Notifier.Model;

namespace Notifier.Controller
{
    public class NotificationController : IController<INotificationModel>
    {
        private Timer mTimer = new Timer();

        public NotificationController()
        {
            mTimer.AutoReset = false;
            Interval = 1000 * 60;
            mTimer.Elapsed += mTimer_Elapsed;
        }

        public bool IsMonitoring
        {
            get { return mTimer.Enabled; }
        }

        //
        // Summary:
        //     Gets or sets the interval at which to raise the System.Timers.Timer.Elapsed
        //     event.
        //
        // Returns:
        //     The time, in milliseconds, between raisings of the System.Timers.Timer.Elapsed
        //     event. The default is 100 milliseconds.
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The interval is less than or equal to zero.
        public double Interval
        {
            get;
            set;
        }

        public INotificationModel Model
        {
            get;
            set;
        }

        public void Start()
        {
            if (Model != null)
            {
                mTimer.Interval = Interval;
                mTimer.Start();
            }
        }

        public void Stop()
        {
            mTimer.Stop();
        }

        private void mTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Model.Update();
            mTimer.Interval = Interval;
            mTimer.Start();
        }
    }
}