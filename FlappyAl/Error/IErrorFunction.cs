namespace FlappyAl
{
    public interface IErrorFunction
    {
        /// <summary>
        /// Evaluates the error between two different vectors.
        /// </summary>
        /// <param name="expected">The expected vector. Same size as <paramref name="actual"/>.</param>
        /// <param name="actual">The actual vector. Same size as <paramref name="expected"/>.</param>
        /// <returns>The error.</returns>
        double Evaluate(double[] expected, double[] actual);

        /// <summary>
        /// Evaluates the error between two different vectors and its partial derivate with respect to each element of the actual vector.
        /// </summary>
        /// <param name="expected">The expected vector. Same size as <paramref name="actual"/>.</param>
        /// <param name="actual">The actual vector. Same size as <paramref name="expected"/>.</param>
        /// <param name="actual">The target for the partial derivative vector. Same size as <paramref name="actual"/>.</param>
        /// <returns>The error.</returns>
        double Derivative(double[] expected, double[] actual, double[] derivative);
    }
}
