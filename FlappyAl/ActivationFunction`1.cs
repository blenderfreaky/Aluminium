using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{
    public interface IActivationFunction<T>
    {
        T Activate(T val);
        T Deactivate(T val);
        T Derivative(T val);
    }
}
