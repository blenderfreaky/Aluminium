using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyAl
{

    public class LeakyReLU : IActivationFunction<double>
    {
        public double Leakyness;

        public LeakyReLU(double leakyness = 0.5d) => Leakyness = leakyness;

        public double Activate(double val) => val >= 0 ? val : val * Leakyness;
        public double Deactivate(double val) => val >= 0 ? val : val / Leakyness;
        public double Derivative(double val) => val >= 0 ? 1 : Leakyness;
    }
}
