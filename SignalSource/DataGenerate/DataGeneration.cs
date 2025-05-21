using System.Net.Sockets;

namespace SignalSource.DataGenerate
{
    internal class DataGeneration
    {
        public double FirstTypeDataGeneration()
        {
            var random = new Random();
            var doubleDataGeneration = random.NextDouble();
            return doubleDataGeneration;
        }

        public byte[] SecondTypeDataGeneration()
        {
            var random = new Random();
            var randomLenght = random.Next(1, 1000);
            var secondTypeData = new byte[randomLenght];
            random.NextBytes(secondTypeData);
            return secondTypeData;
        }
    }
}
