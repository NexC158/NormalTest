using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using SignalRecieverAnalyzer.Connection;


namespace SignalRecieverAnalyzer.Data
{
    internal class DataFilter
    {
        private static uint _bufferSize = 13;

        private static int _size = 4;

        private DataAnalyze _analyze = new();

        private readonly object _lock = new object();

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }

        public async Task DataFilterAsync(ClientConnection connectionSocket, int currentId) // Думаю обработку надо будет делать чанками как в майнкрафте
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
                    Console.WriteLine($"Сработал catch в методе ProcessDataAsync | Клиент {currentId} | Ошибка | {ex.Message}");
                    throw;
                }

                double doubleValue = -1;

                byte type1 = (byte)DataTypes.first; // 0

                byte type2 = (byte)DataTypes.second; // 1

                var size = BitConverter.ToUInt32(buffer.AsSpan(0, _size)); 

                if (size == _bufferSize && buffer[4] == type1)
                {
                    doubleValue = BitConverter.ToDouble(buffer, 5);

                    await _analyze.ProcessDataAsync(doubleValue, currentId);

                   

                //await _analyze.ProcessDataAsync(doubleValue, currentId);


                Console.WriteLine($"Сработал метод ProcessDataAsync | Клиент {currentId} | Принято {size} байт | Полученное значение {doubleValue}");
                }
                else if (size < _bufferSize)
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Шум Клиент {currentId} | Размер шума {size} ");
                    
                }
                else
                {
                    Console.WriteLine($"Сработал метод ProcessDataAsync | Шум Клиент {currentId} | Размер шума {size} | Делаю буфер длиной {size - buffer.Length}");

                    var buffer1 = new byte[size - buffer.Length];

                    received = await connectionSocket._connectionToServerSocket.ReceiveAsync(buffer1, 0);

                    if (buffer[4] == type2)
                    {
                        Console.WriteLine($"Сработал метод ProcessDataAsync | Прочитал весь шум | Клиент {currentId} | Все четко продолжаем");
                    }
                    else
                    {
                        Console.WriteLine($"Сработал метод ProcessDataAsync | Шум | Клиент {currentId} | Что-то тут нечисто");
                    }

                }

                
            //return doubleValue;
            
        } 
    }
}// написать каунтер 




