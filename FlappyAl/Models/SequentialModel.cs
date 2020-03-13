namespace Aluminium.Models
{
    using Aluminium.Error;
    using Aluminium.Layers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate double[] ArraySource(int length);

    public class SequentialModel
    {
        public IReadOnlyList<ILayer> Layers { get; }

        public int InputSize => Layers[0].InputSize;
        public int OutputSize => Layers[Layers.Count - 1].OutputSize;

        public ArraySource ArraySource { get; }

        public SequentialModel(params ILayer[] layers) : this(layers, null) { }
        public SequentialModel(ArraySource arraySource, params ILayer[] layers) : this(layers, arraySource) { }

        public SequentialModel(IReadOnlyList<ILayer> layers, ArraySource? arraySource = null)
        {
            Layers = layers;
            ArraySource = arraySource ?? (size => new double[size]);
        }

        public double[] Evaluate(double[] input)
        {
            if (input.Length != InputSize) throw new ArgumentException("Expected vector to be of size " + InputSize + ".", nameof(input));

            var current = input;

            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var output = ArraySource(layer.OutputSize);
                layer.Evaluate(current, output);
            }

            return current;
        }

        public double Train(double[] input, double[] expectedOutput, double[]? actualOutput, double learningRate, IErrorFunction errorFunction)
        {
            if (input.Length != InputSize) throw new ArgumentException("Expected vector to be of size " + InputSize + ".", nameof(input));
            if (expectedOutput.Length != OutputSize) throw new ArgumentException("Expected vector to be of size " + OutputSize + ".", nameof(expectedOutput));

            var vectors = new double[Layers.Count + 1][];

            vectors[0] = input;

            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];

                if (actualOutput == null || i != Layers.Count - 1) vectors[i + 1] = ArraySource(layer.OutputSize);
                else vectors[i + 1] = actualOutput;

                layer.Evaluate(vectors[i], vectors[i + 1]);
            }

            var output = vectors[vectors.Length - 1];

            if (output.Length != OutputSize) throw new InvalidOperationException("Internal error.");

            var outputErrorSignal = ArraySource(OutputSize);
            var error = errorFunction.Derivative(expectedOutput, output, outputErrorSignal);

            for (int i = Layers.Count - 1; i >= 0; i--)
            {
                var layer = Layers[i];
                var inputErrorSignal = ArraySource(layer.InputSize);
                layer.Train(vectors[i], vectors[i + 1], outputErrorSignal, inputErrorSignal, learningRate);
                outputErrorSignal = inputErrorSignal;
            }

            return error;
        }

        public void UseTraining()
        {
            foreach (var layer in Layers) layer.UseTraining();
        }

        public double Train(
            Func<(double[] Input, double[] ExpectedOutput)> dataSource,
            int batchSize, int sampleSize,
            double learningRate,
            IErrorFunction errorFunction,
            Action<int, double, double?>? callback = null,
            Func<double[], double[], double>? metric = null)
        {
            var actualOutput = new double[OutputSize];
            var meanError = 0d;

            for (int i = 0; i < batchSize; i++)
            {
                var totalMetric = 0d;

                Parallel.For(0, sampleSize, j =>
                {
                    var (input, expectedOutput) = dataSource();

                    meanError += Train(input, expectedOutput, actualOutput, learningRate, errorFunction);

                    totalMetric += metric?.Invoke(expectedOutput, actualOutput) ?? double.NaN;
                });

                UseTraining();
                callback?.Invoke(i, meanError / (i + 1), double.IsNaN(totalMetric) ? (double?)null : totalMetric / sampleSize);
            }

            return meanError / batchSize;
        }
    }
}
