﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlappyAl
{

    public class TanH : IActivationFunction
    {
        public TanH() { }

        public void Evaluate(double[] input, double[] output)
        {
            if (input.Length != output.Length) throw new ArgumentException("Must be same size as " + nameof(input) + ".", nameof(output));

            for (int i = 0; i < input.Length; i++)
            {
                var val = input[i];
                output[i] = 1d / (1d + Math.Exp(-val));
            }
        }

        public void Derivative(double[] input, double[] output)
        {
            if (input.Length != output.Length) throw new ArgumentException("Must be same size as " + nameof(input) + ".", nameof(output));

            for (int i = 0; i < input.Length; i++)
            {
                var val = input[i];
                //var computed = 1d / (1d + Math.Exp(-val));
                var derivative = val * (1d - val);
                output[i] = derivative;// * val;
            }
        }
    }
}