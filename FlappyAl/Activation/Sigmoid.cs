namespace Aluminium.Activation
{
    using System;

    public class Sigmoid : IActivationFunction
    {
        private double Evaluate(double val) => 1d / (1d + Math.Exp(-val));

        public void Evaluate(double[] input, double[] output)
        {
            if (input.Length != output.Length) throw new ArgumentException("Must be same size as " + nameof(input) + ".", nameof(output));

            for (int i = 0; i < input.Length; i++)
            {
                var val = input[i];
                output[i] = Evaluate(val);
            }
        }

        public void Deactivate(double[] inputs, double[] outputs, double[] outputErrorSignal, double[] inputErrorSignal)
        {
            if (outputs.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(outputs));
            if (outputErrorSignal.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(outputErrorSignal));
            if (inputErrorSignal.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(inputErrorSignal));

            for (int i = 0; i < outputErrorSignal.Length; i++)
            {
                var errorSignal = outputErrorSignal[i];
                var sigmoid = outputs[i]; // = sigmoid(inputs[i])

                var derivative = sigmoid * (1d - sigmoid);
                inputErrorSignal[i] = derivative * errorSignal;
            }
        }
    }
}
