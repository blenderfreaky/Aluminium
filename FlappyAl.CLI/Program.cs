namespace Aluminium.CLI
{
    using Aluminium.Activation;
    using Aluminium.CLI.Properties;
    using Aluminium.Datasets;
    using Aluminium.Error;
    using Aluminium.Layers;
    using Aluminium.Models;
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;
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
                //new ActivationLayer(new Sigmoid(), 800),
                new ActivationLayer(new LeakyReLU(0.5d), 800),
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
                            image[(i * mnistImage.Width) + j] = mnistImage.Image[i, j] / 255d;
                        }
                    }

                    var label = new double[10];

                    label[mnistImage.Label] = 1;

                    return (image, label);
                }).ToArray();

            var rng = new Random();
            (double[] Input, double[] ExpectedOutput) dataSource() => mnistTrain[rng.Next(0, mnistTrain.Length - 1)];

            //using var window = new RenderWindow(new VideoMode(600, 600), "Map");
            //window.Clear(Color.Black);
            //var (exampleInput, exampleExpectedOutput) = dataSource();
            //var img = new Image(28, 28);
            //for (uint i = 0; i < 28; i++)
            //{
            //    for (uint j = 0; j < 28; j++)
            //    {
            //        var brightness = (byte)(exampleInput[(j * 28) + i] * 255d);
            //        img.SetPixel(i, j, new Color(brightness, brightness, brightness));
            //    }
            //}
            //window.Draw(new Sprite(new Texture(img)) { Scale = new Vector2f(600 / 28f, 600 / 28f) });
            //window.Draw(new Text("Actual: " + Array.IndexOf(exampleExpectedOutput, 1), new Font(Resources.Arial)));
            //window.Display();

            model.Train(
                dataSource: dataSource,
                epochs: 60000,
                batchSize: 1000,
                learningRate: 0.001d,
                errorFunction: new MeanSquareError(),
                callback: (i, error, metric) => Console.WriteLine("[" + i.ToString().PadLeft(5) + "] (" + (metric * 100d)?.ToString("0.00").PadLeft(6) + "%) Error: " + error),
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
                            );

            var rng = new Random();

            var errorFunction = new MeanSquareError();

            model.Train(
                dataSource: () =>
                {
                    var a = rng.NextDouble() * 10;
                    var b = rng.NextDouble() * 10;
                    var c = rng.NextDouble() * 10;
                    return (new double[] { a, b, c }, new double[] { a + b + c });
                },
                epochs: 10000,
                batchSize: 1000,
                learningRate: 0.0001d,
                errorFunction: new MeanSquareError(),
                callback: (i, error, _) => Console.WriteLine("[" + i + "] Error: " + error));
        }
    }
}
