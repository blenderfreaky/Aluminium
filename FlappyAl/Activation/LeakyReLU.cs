namespace Aluminium.Activation
{
    using System;

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

        public void Deactivate(double[] inputs, double[] outputs, double[] outputErrorSignal, double[] inputErrorSignal)
        {
            if (outputs.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(outputs));
            if (outputErrorSignal.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(outputErrorSignal));
            if (inputErrorSignal.Length != inputs.Length) throw new ArgumentException("Must be same size as " + nameof(inputs) + ".", nameof(inputErrorSignal));

            for (int i = 0; i < outputErrorSignal.Length; i++)
            {
                var errorSignal = outputErrorSignal[i];
                var input = inputs[i];

                var derivative = input < 0 ? Leakyness : 1;
                inputErrorSignal[i] = derivative * errorSignal;
            }
        }
    }
}
