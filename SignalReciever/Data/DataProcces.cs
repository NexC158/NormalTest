using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using SignalRecieverAnalyzer.Connection;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    internal class DataProcces
    {
        private static uint _bufferSize = 13;

        private static int _size = 4;

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }
        
        public async Task<double> ProcessDataAsync(ClientConnection connectionSocket, int currentId) // работает для не large объектов. Думаю обработку надо будет делать чанками как в майнкрафте
        {
            byte[] buffer = new byte[_bufferSize];

            int received = 0;

            try
            {
                received = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer, 0);
                if (received == 0)
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Клиент {currentId} | Ошибка приема {received} переданных байт");

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

                //Console.WriteLine($"Сработал метод ProcessDataAsync | Клиент {currentId} | Принято {size} байт | Полученное значение {BitConverter.ToDouble(buffer, 5)}");
            }
            else if (size < 13)
            {
                Console.WriteLine($"Сработал метод ProcessDataAsync | Шум Клиент {currentId} | Размер шума {size} ");

                await ProcessDataAsync(connectionSocket, currentId);
            }
            else
            {
                Console.WriteLine($"Сработал метод ProcessDataAsync | Шум Шум Клиент {currentId} | Размер шума {size} | Делаю буфер длиной {size - buffer.Length}");

                var buffer1 = new byte[size - buffer.Length];

                received = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer1, 0);

                if (buffer[4] == type2)
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Шум | Клиент {currentId} | Все четко продолжаем");
                }
                else
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Шум | Клиент {currentId} | Что-то тут нечисто");
                }

                await ProcessDataAsync(connectionSocket, currentId);
            }

            return doubleValue;
        }
    }
}
