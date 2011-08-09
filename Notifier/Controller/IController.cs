using System;
using System.Collections.Generic;
using System.Text;
using Notifier.Model;

namespace Notifier.Controller
{
    interface IController<T>
        where T : IModel
    {
        T Model { get; set; }
    }
}
