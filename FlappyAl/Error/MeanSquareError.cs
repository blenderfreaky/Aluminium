namespace Aluminium.Error
{
    using System;

    public class MeanSquareError : IErrorFunction
    {
        public double Evaluate(double[] expected, double[] actual)
        {
            if (expected.Length != actual.Length) throw new ArgumentException("Must be same size as " + nameof(expected) + ".", nameof(actual));

            double accumulator = 0d;

            for (int i = 0; i < expected.Length; i++)
            {
                var absoluteError = expected[i] - actual[i];
                accumulator += absoluteError * absoluteError;
            }

            return accumulator / expected.Length;
        }

        public double Derivative(double[] expected, double[] actual, double[] derivative)
        {
            if (expected.Length != actual.Length) throw new ArgumentException("Must be same size as " + nameof(expected) + ".", nameof(actual));
            if (derivative.Length != actual.Length) throw new ArgumentException("Must be same size as " + nameof(actual) + ".", nameof(derivative));

            double accumulator = 0d;

            for (int i = 0; i < expected.Length; i++)
            {
                var absoluteError = expected[i] - actual[i];
                derivative[i] = absoluteError;

                accumulator += absoluteError * absoluteError;
            }

            return accumulator / expected.Length;
        }
    }
}
