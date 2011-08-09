
namespace Notifier.Model.Monitor
{
    public abstract class OverLimitMonitor : Monitor
    {
        public override string Caption
        {
            get { return string.Format(FormatString, Current - Limit); }
        }

        public int Current { get; protected set; }

        public int Limit { get; set; }

        public override bool ShouldNotify
        {
            get { return Current > Limit; }
        }
    }
}
