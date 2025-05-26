using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using SignalRecieverAnalyzer.Connection;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    internal class DataProccesing
    {
        private int _bufferSize = 13;

        double[] result = new double[3];

        private int _currentClientORConnect = 0;

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }

        public async Task<double> RecieveDataAsync(ClientConnection connectionSocket) // вот тут надо вместо Task<string> сделать double[] or List<double>. Сейчас string потому что это первое пришло в голову и не надо писать логику анализатора 
        {
            byte[] buffer1 = new byte[_bufferSize];

            byte[] buffer2 = new byte[_bufferSize];

            StringBuilder sb = new StringBuilder();

            double result = 0;

            int received1 = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer1, 0); // вот тут надо сделать проверку на второй тип данных

            if (buffer1.Length >=5)
            {
                result = ProcessBuffer(buffer1, received1);
            }
            else
            {
                Console.WriteLine("RecieveDataAsync | Фигня какая-то");
            }
            
            return result; // придумать как обойти ошибку с реальным возвратом 0 и стандартным
        }

        private double ProcessBuffer(byte[] buffer, int receivedBytes)
        {
            double receivedDouble = 0;

            if (buffer[4] == (byte)DataTypes.first && receivedBytes == 13)
            {
                receivedDouble = BitConverter.ToDouble(buffer, 5);

                Console.WriteLine($"ProcessBuffer | Получен первый тип данных: {receivedDouble}");
                
            }
            else if (buffer[4] == (byte)DataTypes.second)
            {
                Console.WriteLine("ProcessBuffer | Получен второй тип данных - пропускаем");
            }
            else
            {
                Console.WriteLine("ProcessBuffer | Не могу прочитать buffer");
            }
            return receivedDouble;
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
