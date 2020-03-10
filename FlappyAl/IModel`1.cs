using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{

    public interface IModel<T>
    {
        T[] Evaluate(T[] input);
        void Train(double[] errors, double stepSize);

        int InputSize { get; }
        int OutputSize { get; }

        double[] Output { get; }
    }
}
