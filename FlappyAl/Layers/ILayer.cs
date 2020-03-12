using System;
using System.Collections.Generic;
using System.Text;

namespace FlappyAl
{
    public interface ILayer
    {
        int InputSize { get; }
        int OutputSize { get; }

        void Evaluate(double[] input, double[] output);

        void Train(
            double[] inputs, double[] outputs,
            double[] outputErrorSignal, double[] inputErrorSignal,
            double learningRate);

        void UseTraining();
    }
}
