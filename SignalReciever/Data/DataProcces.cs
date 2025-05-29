using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using SignalRecieverAnalyzer.Connection;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    internal class DataProcces
    {
        private static int _bufferSize = 13;

        private static int _size = 4;

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }
        
        public async Task<double> ProcessDataAsync(ClientConnection connectionSocket)
        {
            byte[] buffer = new byte[_bufferSize];

            int received = 0;

            try
            {
                received = connectionSocket._connectionToServerSocket.Receive(buffer, 0);
                if (received == 0)
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Ошибка приема {received} переданных байт");

                    throw new SocketException(10054);
                }
            }
            catch (SocketException ex) when (ex.ErrorCode == 10054)
            {
                throw;
            }

            double doubleValue = -1;

            byte type1 = (byte)DataTypes.first;

            byte type2 = (byte)DataTypes.second;

            var sizeBytes = new byte[_size];

            Buffer.BlockCopy(buffer, 0, sizeBytes, 0, _size);

            var size = BitConverter.ToUInt32(sizeBytes);

            if (size == 13 && buffer[4] == type1)
            {
                doubleValue = BitConverter.ToDouble(buffer, 5);

                Console.WriteLine($"Сработал метод ProcessDataAsync | Принято {size} байт | Полученное значение {BitConverter.ToDouble(buffer, 5)}");
            }
            else if (size < 13)
            {
                Console.WriteLine($"Сработал метод ProcessDataAsync | Шум | Размер шума {size} ");

                await ProcessDataAsync(connectionSocket);
            }
            else
            {
                Console.WriteLine($"Сработал метод ProcessDataAsync | Шум | Размер шума {size} | Делаю буфер длиной {size - buffer.Length}");

                var buffer1 = new byte[size - buffer.Length];

                received = connectionSocket._connectionToServerSocket.Receive(buffer1, 0);

                if (buffer[4] == type2)
                {
                    Console.WriteLine("Сработал метод ProcessDataAsync | Все четко продолжаем");
                }
                else
                {
                    Console.WriteLine("Сработал метод ProcessDataAsync | Что-то тут нечисто");
                }

                await ProcessDataAsync(connectionSocket);
            }

            return doubleValue;
        }
    }
}
