using System;
using System.Collections.Generic;

namespace Notifier.Model
{
    public interface IModel
    {
        event EventHandler ContentsUpdated;
    }
}
