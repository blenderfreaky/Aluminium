using System;

namespace FlappyAl.CLI
{
    static class Program
    {
        static void Main(string[] args)
        {
            IModel<double> layer = new InputLayer(1);
            layer = new DenseLayer(layer, 1);
            var model = new OutputLayer(layer, ErrorFunctions.MeanSquaredError);

            var rng = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var first = rng.NextDouble() * 5;
                var second = rng.NextDouble() * 5;

                double[] input = new double[] { first, second };
                double[] expectedOutput = new double[] { first + second };

                model.Train(input, expectedOutput, 0.1, out var actualOutput);

                //Console.WriteLine("In: " + string.Join(", ", input));
                //Console.WriteLine("Out (Expected): " + string.Join(", ", expectedOutput));
                //Console.WriteLine("Out (Actual): " + string.Join(", ", actualOutput));
            }
        }
    }
}
