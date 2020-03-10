using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{

    public interface ILayer<T> : IModel<T>
    {
        IModel<T> ParentLayer { get; }
    }
}
