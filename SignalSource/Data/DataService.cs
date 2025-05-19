using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.Data
{
    internal class DataService : IDataService
    {
        public async Task DataSendAsync(Socket client, double rndDoubleValue)
        {
            byte[] data = BitConverter.GetBytes(rndDoubleValue);

            var sent = await client.SendAsync(data);
            if (sent == 0) throw new Exception("Error data transfer");
            
        }

        public double FirstTypeDataGeneration()
        {
            var random = new Random();
            var doubleDataGeneration = random.NextDouble();
            return doubleDataGeneration;
        }

        public double[] SecondTypeDataGeneration()
        {
            var random = new Random();
            var randomLenght = random.Next(1, 1000);
            var secondTypeData = new double[randomLenght];
            for (int i = 0; i < secondTypeData.Length; i++)
            {
                secondTypeData[i] = random.NextDouble();
            }
            return secondTypeData;
        }
    }
}
