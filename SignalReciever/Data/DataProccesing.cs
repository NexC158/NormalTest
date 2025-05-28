using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using SignalRecieverAnalyzer.Connection;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    internal class DataProccesing
    {
        private int _bufferSize = 13;

        private object _lock = new object();

        //double[] result = new double[3];

        private int _currentClientORConnect = 0;

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }
        // вот тут надо вместо Task<string> сделать double[] or List<double>
        public async Task<double> RecieveDataAsync(ClientConnection connectionSocket, int connectedId) // надо сделать скользящее окно для буферов, но пока не знаю как
        {
            var buffer1 = new byte[_bufferSize];

            var buffer2 = new byte[_bufferSize];

            double result = -1;
            double result2 = -1;

            var received1 = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer1, 0);
            var received2 = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer2, 0);
            lock (_lock)
            {
                result = ProcessBuffer(buffer1, received1, connectedId);

                result2 = ProcessBuffer(buffer2, received2, connectedId);
                Console.WriteLine($"Сработал RecieveDataAsync | Вызов ProcessBuffer с параметрами: result {result} | received1 {received1} | connectedId {connectedId}");
            }


            return result;

        }

        private double ProcessBuffer(byte[] buffer, int receivedBytes, int connectedId)
        {
            double receivedDouble = 0;

            if (buffer[4] == (byte)DataTypes.first && receivedBytes == 13)
            {
                receivedDouble = BitConverter.ToDouble(buffer, 5);

                Console.WriteLine($"Сработал ProcessBuffer | Клиент {connectedId} получил первый тип данных: {receivedDouble}");

            }
            else if (buffer[4] == (byte)DataTypes.second)
            {
                Console.WriteLine($"Сработал ProcessBuffer | Клиент {connectedId} получил второй тип данных - пропускаем");
            }
            else
            {
                Console.WriteLine("Сработал ProcessBuffer | Не могу прочитать buffer");
            }
            return receivedDouble;
        }

        // Вот тут находится метод, именно из-за дебага мини сервера-клиента я понял в чем ошибки и как сделать ананлиз
        // он конечно еще сырой, но я понял очень много, половина вопросов отпала

        private static double Recieve(Socket socket)
        {
            byte[] buffer = new byte[13];

            double doubleValue = -1;

            byte type1 = (byte)DataTypes.first;

            byte type2 = (byte)DataTypes.second;

            var receive = socket.Receive(buffer, 0);

            var sizeBytes = new byte[4];

            Buffer.BlockCopy(buffer, 0, sizeBytes, 0, 4);

            var size = BitConverter.ToUInt32(sizeBytes);

            if (size == 13 && buffer[4] == type1)
            {
                doubleValue = BitConverter.ToDouble(buffer, 5);
                Console.WriteLine($"Сработал метод Receive | Принято {size} байт | Полученное значение {BitConverter.ToDouble(buffer, 5)}");
            }
            else
            {

                Console.WriteLine($"Сработал метод Receive | Шум | Размер шума {size} | Передвигаю буфер на позицию {size - 13}");

                int shiftRead = (int)size - 13;

                var buffer1 = new byte[shiftRead];

                receive = socket.Receive(buffer1, 0);

                Recieve(socket);
            }


            return doubleValue;
        }
    }
}
