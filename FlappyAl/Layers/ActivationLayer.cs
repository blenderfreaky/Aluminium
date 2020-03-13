using Aluminium.Activation;

namespace Aluminium.Layers
{
    public class ActivationLayer : ILayer
    {
        public IActivationFunction ActivationFunction { get; }

        public int Size { get; }
        int ILayer.InputSize => Size;
        int ILayer.OutputSize => Size;

        public ActivationLayer(IActivationFunction activationFunction, int size = -1)
        {
            ActivationFunction = activationFunction;
            Size = size;
        }

        public void Evaluate(double[] input, double[] output)
        {
            ActivationFunction.Evaluate(input, output);
        }

        public void Train(
            double[] inputs, double[] outputs,
            double[] outputErrorSignal, double[] inputErrorSignal,
            double learningRate)
        {
            ActivationFunction.Derivative(outputErrorSignal, inputErrorSignal);
        }

        public void UseTraining() { }
    }
}
