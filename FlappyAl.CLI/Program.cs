namespace Aluminium.CLI
{
    using Aluminium.Activation;
    using Aluminium.Datasets;
    using Aluminium.Error;
    using Aluminium.Layers;
    using Aluminium.Models;
    using System;
    using System.Linq;

    static class Program
    {
        static void Main(string[] args)
        {
            //AdditionTest();
            MnistTest();
        }

        private static void MnistTest()
        {
            var model = new SequentialModel(
                new DenseLayer(784, 800),
                new ActivationLayer(new LeakyReLU(0.5), 800),
                new DenseLayer(800, 10),
                new ActivationLayer(new Sigmoid(), 10)
                );

            var mnistTrain = MnistReader.ReadTrainingData()
                .Select(mnistImage =>
                {
                    var image = new double[mnistImage.Width * mnistImage.Height];

                    for (int i = 0; i < mnistImage.Height; i++)
                    {
                        for (int j = 0; j < mnistImage.Width; j++)
                        {
                            image[(i * mnistImage.Width) + j] = mnistImage.Image[i, j];
                        }
                    }

                    var label = new double[10];

                    label[mnistImage.Label] = 1;

                    return (image, label);
                }).ToArray();

            var rng = new Random();

            model.Train(
                dataSource: () => mnistTrain[rng.Next(0, mnistTrain.Length - 1)],
                batchSize: 60000,
                sampleSize: 50,
                learningRate: 0.1d,
                errorFunction: new MeanSquareError(),
                callback: (i, error, metric) => Console.WriteLine("[" + i + "] (" + (metric * 100d)?.ToString("0.00") + "%) Error: " + error),
                metric: (expected, actual) =>
                {
                    if (expected.Length == 0) throw new ArgumentException("Expected array to be non-empty.", nameof(expected));
                    if (expected.Length != actual.Length) throw new ArgumentException("Expected array of same size as " + nameof(expected) + ".", nameof(actual));

                    var maxExpected = 0;
                    var maxActual = 0;

                    for (int i = 1; i < expected.Length; i++)
                    {
                        if (expected[i] > expected[maxExpected]) maxExpected = i;
                        if (actual[i] > actual[maxActual]) maxActual = i;
                    }

                    return maxExpected == maxActual ? 1 : 0;
                });
        }

        private static void AdditionTest()
        {
            var model = new SequentialModel(
                            new DenseLayer(3, 1)
                            //new ActivationLayer(new LeakyReLU(0.5), 5),
                            //new DenseLayer(5, 1)
                            //new ActivationLayer(new LeakyReLU(-0.5), 1)
                            );

            var rng = new Random();

            var errorFunction = new MeanSquareError();

            model.Train(
                dataSource: () => {
                    var a = rng.NextDouble() * 10;
                    var b = rng.NextDouble() * 10;
                    var c = rng.NextDouble() * 10;
                    return (new double[] { a, b, c }, new double[] { a + b + c });
                },
                batchSize: 10000,
                sampleSize: 100,
                learningRate: 0.0001d,
                errorFunction: new MeanSquareError(),
                callback: (i, error, _) => Console.WriteLine("[" + i + "] Error: " + error));
        }
    }
}
