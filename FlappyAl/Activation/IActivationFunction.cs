namespace Aluminium.Activation
{
    public interface IActivationFunction
    {
        void Evaluate(double[] input, double[] output);
        void Deactivate(double[] input, double[] output);
    }
}
