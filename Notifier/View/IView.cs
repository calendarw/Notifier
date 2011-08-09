using Notifier.Model;

namespace Notifier.View
{
    public interface IView<T>
        where T : IModel
    {
        T Model { get; set; }
    }
}
