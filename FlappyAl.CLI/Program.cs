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

            model.Train(() => mnistTrain[rng.Next(0, mnistTrain.Length - 1)], 10000, 10, 10.0d,
                new MeanSquareError(), (i, error) => Console.WriteLine("[" + i + "] Error: " + error));
        }

        private static void AdditionTest()
        {
            var model = new SequentialModel(
                            new DenseLayer(3, 5),
                            new ActivationLayer(new LeakyReLU(0.5), 5),
                            new DenseLayer(5, 1)
                            );

            var rng = new Random();

            var errorFunction = new MeanSquareError();

            var weightedMeanError = 0d;

            for (int i = 0; i < 100000; i++)
            {
                var rng0 = rng.NextDouble() * 10;
                var rng1 = rng.NextDouble() * 10;
                var rng2 = rng.NextDouble() * 10;

                double[] input = new double[] { rng0, rng1, rng2 };
                double[] expectedOutput = new double[] { rng0 + rng1 + rng2 };
                double[] actualOutput = new double[1];

                var error = model.Train(input, expectedOutput, actualOutput, 0.00001, errorFunction);

                weightedMeanError = (weightedMeanError * 0.9) + (error * 0.1);

                if (i % 1 == 0) model.UseTraining();

                Console.WriteLine("Error: " + error + " Weighted Mean: " + weightedMeanError);
                //Console.WriteLine("In: " + string.Join(", ", input));
                //Console.WriteLine("Out (Expected): " + string.Join(", ", expectedOutput));
                //Console.WriteLine("Out (Actual): " + string.Join(", ", actualOutput));
            }
        }
    }
}
