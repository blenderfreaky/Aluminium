using System;

namespace FlappyAl
{

    public static class ErrorFunctions
    {
        public static ErrorFunction<double> MeanSquaredError = Mean((a, e) => (a - e) * (a - e));

        private static ErrorFunction<double> Mean(Func<double, double, double> func) =>
            (a, e) =>
            {
                if (a.Length != e.Length) throw new ArgumentException("Expected arguments to be of same size.");

                var accumulator = 0d;

                for (int i = 0; i < a.Length; i++)
                {
                    accumulator += func(a[i], e[i]);
                }

                return accumulator / a.Length;
            };
    }
}
