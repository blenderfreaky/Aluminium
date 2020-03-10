using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlappyAl
{

    public class OutputLayer : ILayer<double>, ITrainable<double>
    {
        public IModel<double> ParentLayer { get; }

        public int InputSize => ParentLayer.OutputSize;
        public int OutputSize => ParentLayer.OutputSize;

        public ErrorFunction<double> ErrorFunction { get; }

        public double[] Output => ParentLayer.Output;

        public OutputLayer(IModel<double> parentLayer, ErrorFunction<double> errorFunction)
        {
            ParentLayer = parentLayer;
            ErrorFunction = errorFunction;
        }

        public double[] Evaluate(params double[] input) => ParentLayer.Evaluate(input);
        public void Train(double[] errors, double stepSize) => ParentLayer.Train(errors, stepSize);
        public void Train(double[] input, double[] expectedOutput, double stepSize, out double[] actualOutput)
        {
            actualOutput = ParentLayer.Evaluate(input);
            if (actualOutput.Length != expectedOutput.Length) throw new ArgumentException("Expected array of size " + actualOutput.Length + ".", nameof(expectedOutput));

            var errors = new double[actualOutput.Length];
            for (int i = 0; i < errors.Length; i++) errors[i] = ErrorFunction(actualOutput[i], expectedOutput[i]);

            ParentLayer.Train(errors, stepSize);
        }
    }
}
