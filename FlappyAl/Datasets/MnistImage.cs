namespace FlappyAl.Datasets
{
    public class MnistImage
    {
        public int Width { get; }
        public int Height { get; }

        public byte Label { get; }
        public byte[,] Image { get; }

        public MnistImage(int width, int height, byte label, byte[,] image)
        {
            Width = width;
            Height = height;
            Label = label;
            Image = image;
        }
    }
}
