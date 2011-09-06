using Notifier.View;
using NUnit.Framework;

namespace NotifierTest.View
{
    [TestFixture]
    public class NotifyIconBalloonViewTest : NotificationViewTest
    {
        protected override INotificationView GetView()
        {
            return new NotifyIconBalloon();
        }
    }
}
