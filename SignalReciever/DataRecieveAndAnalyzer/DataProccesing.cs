using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    public class DataProccesing : IDataProccesing
    {

        public async Task<double> RecieveDoubleAsync(Socket socket)
        {
            byte[] buffer = new byte[11];

            int received = await socket.ReceiveAsync(buffer, 0);

            return BitConverter.ToDouble(buffer, 3);
        }

        public async Task<byte[]> Analyzer(Socket socket1, Socket socket2)
        {
            int shiftId = 0;
            byte[] resultBytes = new byte[20];
            var data1 = await RecieveDoubleAsync(socket1);
            var data2 = await RecieveDoubleAsync(socket2);

            var dataToBytes1 = BitConverter.GetBytes(data1);
            var dataToBytes2 = BitConverter.GetBytes(data2);

            if (Math.Abs(data1 - data2) < 0.5)
            {
                var bytesShiftId = BitConverter.GetBytes(shiftId);

                for (int i = 0; i < resultBytes.Length; i++)
                {
                    resultBytes[i] = dataToBytes1[i];
                }
            }

            else if (Math.Abs(data1 - data2) > 0.5)
            {
                shiftId++;

            }
            return resultBytes;
        }

        
    }
}
