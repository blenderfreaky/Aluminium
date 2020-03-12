using FlappyAl.Error;
using System;

namespace FlappyAl.CLI
{
    static class Program
    {
        static void Main(string[] args)
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
