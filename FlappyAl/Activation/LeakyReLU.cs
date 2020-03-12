using System;
using System.Collections.Generic;
using System.Text;

namespace FlappyAl
{

    public class LeakyReLU : IActivationFunction
    {
        public double Leakyness { get; }

        public LeakyReLU(double leakyness) => Leakyness = leakyness;

        public void Evaluate(double[] input, double[] output)
        {
            if (input.Length != output.Length) throw new ArgumentException("Must be same size as " + nameof(input) + ".", nameof(output));

            for (int i = 0; i < input.Length; i++)
            {
                var val = input[i];
                output[i] = val < 0 ? Leakyness * val : val;
            }
        }

        public void Derivative(double[] input, double[] output)
        {
            if (input.Length != output.Length) throw new ArgumentException("Must be same size as " + nameof(input) + ".", nameof(output));

            for (int i = 0; i < input.Length; i++)
            {
                var val = input[i];
                var derivative = val < 0 ? Leakyness : 1;
                output[i] = derivative;// * val;
            }
        }
    }
}
