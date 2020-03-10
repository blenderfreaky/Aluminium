using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{
    public interface ITrainable<T>
    {
        T[] Evaluate(T[] input);
        void Train(T[] input, T[] expectedOutput, double stepSize, out T[] actualOutput);
    }
}
