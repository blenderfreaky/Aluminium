using System;
using System.Collections.Generic;
using System.Text;

namespace FlappyAl
{
    public interface IActivationFunction
    {
        void Evaluate(double[] input, double[] output);
        void Derivative(double[] input, double[] output);
    }
}
