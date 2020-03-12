using FlappyAl.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlappyAl.Datasets
{
    public static class MnistReader
    {
        public static IEnumerable<MnistImage> ReadTrainingData()
        {
            foreach (var item in Read(Resources.MnistTrainImages, Resources.MnistTrainLabels))
            {
                yield return item;
            }
        }

        public static IEnumerable<MnistImage> ReadTestData()
        {
            foreach (var item in Read(Resources.MnistTestImages, Resources.MnistTestLabels))
            {
                yield return item;
            }
        }

        private static IEnumerable<MnistImage> Read(byte[] imagesBytes, byte[] labelsBytes  )
        {
            using var labelsStream = new MemoryStream(labelsBytes);
            using var labelsReader = new BinaryReader(labelsStream);

            using var imagesStream = new MemoryStream(imagesBytes);
            using var imagesReader = new BinaryReader(imagesStream);

            int magicNumber = imagesReader.ReadBigInt32();
            int numberOfImages = imagesReader.ReadBigInt32();
            int width = imagesReader.ReadBigInt32();
            int height = imagesReader.ReadBigInt32();

            int magicLabel = labelsReader.ReadBigInt32();
            int numberOfLabels = labelsReader.ReadBigInt32();

            for (int i = 0; i < numberOfImages; i++)
            {
                var bytes = imagesReader.ReadBytes(width * height);
                var arr = new byte[height, width];

                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        arr[j, k] = bytes[(j * width) + k];
                    }
                }

                yield return new MnistImage(width, height, labelsReader.ReadByte(), arr);
            }
        }

        private static int ReadBigInt32(this BinaryReader br)
        {
            var bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
