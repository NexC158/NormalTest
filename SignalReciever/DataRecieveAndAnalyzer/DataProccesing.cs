using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    public class DataProccesing
    {
        enum DataTypes : byte // byte??int
        {
            first,
            second
        }

        public async Task<string> RecieveDataAsync(Socket socket)
        {
            byte[] buffer = new byte[1000000];

            int received = await socket.ReceiveAsync(buffer, 0);// вот тут надо сделать проверку на второй тип данных
            uint primer = 4141230412;
            var primerToBytes = BitConverter.GetBytes(primer);
            StringBuilder sb = new StringBuilder();
            if (buffer[4] == (byte)DataTypes.first)
            {
                
                var size = received;
                sb.Append("Размер: ");
                sb.Append(size);
                sb.Append(" |Тип данных: ");
                sb.Append(" 1");
                sb.Append(" |Payload: ");
                sb.Append(BitConverter.ToDouble(buffer, 5)); // длина
                
            }
            else if (buffer[4] == (byte)DataTypes.second)
            {
                var size = received;
                sb.Append("Размер: ");
                sb.Append(size);
                sb.Append(" |Тип данных: ");
                sb.Append(" 2");
                sb.Append(" |Payload: ");
                sb.Append(BitConverter.ToDouble(buffer, 5));
                //Console.WriteLine("------------пришел шум------------");
                
            }
            return sb.ToString();
        }

        /*public async Task<byte[]> Analyzer(Socket socket1, Socket socket2)
        {
            int shiftId = 0;
            byte[] resultBytes = new byte[1024];
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
                shiftId++; // тут понадобится инкремент
            }
            return resultBytes;
        }*/

        
    }
}
