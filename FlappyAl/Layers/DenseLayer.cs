namespace Aluminium.Layers
{
    using Activation;
    using System;

    public class DenseLayer : ILayer
    {
        public int InputSize { get; }
        public int OutputSize { get; }

        private double[,] Weights { get; }
        private double[,] WeightsAltBuffer { get; }

        public DenseLayer(int inputSize, int outputSize, Func<int, int, double>? weightsInitializer = null)
        {
            InputSize = inputSize;
            OutputSize = outputSize;

            if (weightsInitializer == null)
            {
                var rng = new Random();
                _ = rng; // To silence the compiler
                weightsInitializer = (_, __) => ((rng.NextDouble() * 2) - 1) * 1;
            }

            Weights = new double[InputSize, OutputSize];
            WeightsAltBuffer = new double[InputSize, OutputSize];

            for (int i = 0; i < InputSize; i++)
            {
                for (int j = 0; j < OutputSize; j++)
                {
                    var weight = weightsInitializer(i, j);

                    Weights[i, j] = weight;
                    WeightsAltBuffer[i, j] = weight;
                }
            }
        }

        public DenseLayer(int inputSize, int outputSize, double[,] weights)
        {
            InputSize = inputSize;
            OutputSize = outputSize;

            if (Weights.GetLength(0) != InputSize) throw new ArgumentException($"Expected 2d array of x-size {InputSize}.", nameof(Weights));
            if (Weights.GetLength(1) != OutputSize) throw new ArgumentException($"Expected 2d array of y-size {OutputSize}.", nameof(Weights));

            Weights = weights;
            WeightsAltBuffer = new double[InputSize, OutputSize];
            Array.Copy(Weights, WeightsAltBuffer, Weights.Length);
        }

        public void Evaluate(double[] input, double[] output)
        {
            if (input.Length != InputSize) throw new ArgumentException($"Expected array of size {InputSize}.", nameof(input));
            if (output.Length != OutputSize) throw new ArgumentException($"Expected array of size {OutputSize}.", nameof(output));

            for (int i = 0; i < OutputSize; i++)
            {
                var accumulator = 0d;

                for (int j = 0; j < InputSize; j++)
                {
                    accumulator += input[j] * Weights[j, i];
                }

                output[i] = accumulator;
            }
        }

        public void Train(
            double[] inputs, double[] outputs,
            double[] outputErrorSignal, double[] inputErrorSignal,
            double learningRate)
        {
            if (outputs.Length != OutputSize) throw new ArgumentException($"Expected array of size {OutputSize}.", nameof(outputs));
            if (outputErrorSignal.Length != OutputSize) throw new ArgumentException($"Expected array of size {OutputSize}.", nameof(outputErrorSignal));

            if (inputs.Length != InputSize) throw new ArgumentException($"Expected array of size {InputSize}.", nameof(inputs));
            if (inputErrorSignal.Length != InputSize) throw new ArgumentException($"Expected array of size {InputSize}.", nameof(inputErrorSignal));

            for (int i = 0; i < InputSize; i++)
            {
                var accumulator = 0d;

                for (int j = 0; j < OutputSize; j++)
                {
                    accumulator += outputErrorSignal[j] * Weights[i, j];

                    var errorGradient = outputErrorSignal[j] * inputs[i];
                    WeightsAltBuffer[i, j] += learningRate * errorGradient;
                }

                inputErrorSignal[i] = accumulator;
            }
        }

        public void UseTraining()
        {
            Array.Copy(WeightsAltBuffer, Weights, Weights.Length);
        }
    }
}
