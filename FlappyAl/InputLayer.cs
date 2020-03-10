using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlappyAl
{
    public class InputLayer : IModel<double>
    {
        public int InputSize { get; }

        public int OutputSize => InputSize;

        public double[] Output { get; private set; }

        public InputLayer(int inputSize) => InputSize = inputSize;

        public double[] Evaluate(double[] input) => Output = input;
        public void Train(double[] errors, double stepSize) { }
    }
}
