using System.Windows.Forms;
using Notifier.Model;

namespace Notifier.View
{
    public interface INotificationView : IView<INotificationModel>
    {
        NotifyIcon NotifyIcon { get; set; }
        int Timeout { get; set; }
    }
}