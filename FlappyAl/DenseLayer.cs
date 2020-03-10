using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{
    public class DenseLayer : IModel<double>
    {
        public IModel<double> ParentLayer { get; }

        public int InputSize => ParentLayer.OutputSize;
        public int OutputSize { get; }

        public double[,] Weights { get; }

        public double[] Output { get; private set; }

        public IActivationFunction<double> ActivationFunction { get; }

        public DenseLayer(
            IModel<double> parentLayer,
            int outputSize,
            IActivationFunction<double> activationFunction = null)
        {
            ParentLayer = parentLayer;
            OutputSize = outputSize;
            ActivationFunction = activationFunction ?? new LeakyReLU(0.5);

            Weights = new double[InputSize, OutputSize];

            var rng = new Random();

            for (int i = 0; i < InputSize; i++)
            {
                for (int j = 0; j < OutputSize; j++)
                {
                    Weights[i, j] = (rng.NextDouble() * 2) - 1;
                }
            }
        }

        public double[] Evaluate(double[] input)
        {
            input = ParentLayer.Evaluate(input);

            // TODO: Use array pool
            var output = new double[OutputSize];

            for (int i = 0; i < OutputSize; i++)
            {
                var accumulator = 0d;

                for (int j = 0; j < InputSize; j++)
                {
                    accumulator += input[j] * Weights[j, i];
                }

                output[i] = ActivationFunction.Activate(accumulator);
            }

            return Output = output;
        }

        public void Train(double[] errorSignal, double, double stepSize)
        {
            // TODO: Use array pool
            var inputErrors = new double[InputSize];

            for (int i = 0; i < InputSize; i++)
            {
                var accumulator = 0d;

                for (int j = 0; j < OutputSize; j++)
                {
                    var specificError = ActivationFunction.Deactivate(outputErrors[i]) * Output[j];

                    Weights[i, j] -= specificError * stepSize;

                    accumulator += specificError * Weights[i, j];
                }

                inputErrors[i] = accumulator;
            }

            ParentLayer.Train(inputErrors, stepSize);
        }
    }
}
