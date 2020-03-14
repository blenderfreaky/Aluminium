namespace Aluminium.Activation
{
    public interface IActivationFunction
    {
        void Evaluate(double[] inputs, double[] outputs);
        void Deactivate(double[] inputs, double[] outputs, double[] outputErrorSignal, double[] inputErrorSignal);
    }
}
