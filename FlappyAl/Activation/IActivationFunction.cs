namespace Aluminium.Activation
{
    public interface IActivationFunction
    {
        void Evaluate(double[] input, double[] output);
        void Derivative(double[] input, double[] output);
    }
}
