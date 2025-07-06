using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using SignalRecieverAnalyzer.Connection;


namespace SignalRecieverAnalyzer.Data
{
    internal class DataFilter
    {
        private readonly uint _bufferSize = 13;

        private readonly int _intSizeOfBytesArray = 4;

        enum DataTypes : byte
        {
            first,
            second
        }

        public async IAsyncEnumerable<double> DataFilterAsync(ClientConnection connectionSocket, int currentId, [EnumeratorCancellation] CancellationToken ct) // Думаю обработку надо будет делать чанками как в майнкрафте
        {
            byte[] buffer = new byte[_bufferSize];

            int received = 0;

            while (ct.IsCancellationRequested is false) 
            {
                try
                {
                    received = await connectionSocket.MyReceiveAsync(buffer, 0, ct);
                    if (received == 0)
                    {
                        Console.WriteLine($"Сработал метод DataFilterAsync | Клиент {currentId} | Ошибка приема {received} переданных байт");
                    }
                }
                catch (SocketException ex) when (ex.ErrorCode == 10054)
                {
                    Console.WriteLine($"Сработал catch в методе DataFilterAsync | Клиент {currentId} | Ошибка | {ex.Message}");
                    throw;
                }

                double doubleValue = -1;

                byte type1 = (byte)DataTypes.first; // 0
                byte type2 = (byte)DataTypes.second; // 1
                var size = BitConverter.ToUInt32(buffer.AsSpan(0, _intSizeOfBytesArray));

                if (size == _bufferSize && buffer[4] == type1)
                {
                    doubleValue = BitConverter.ToDouble(buffer, 5);
                    yield return doubleValue;
                    //Console.WriteLine($"Сработал метод DataFilterAsync | Клиент {currentId} | Принято {size} байт | Полученное значение {doubleValue}");
                }
                else if (size < _bufferSize)
                {
                    Console.WriteLine($"Сработал метод DataFilterAsync | Шум Клиент {currentId} | Размер шума {size} ");
                }
                else
                {
                    Console.WriteLine($"Сработал метод DataFilterAsync | Шум Клиент {currentId} | Размер шума {size} | Делаю буфер длиной {size - buffer.Length}");

                    var buffer1 = new byte[size - buffer.Length];
                    received = await connectionSocket.MyReceiveAsync(buffer1, 0, ct);

                }
            }
        }

        //public async IAsyncEnumerable<double> ReadDataAsync(ClientConnection connectionSocket, int currentId, [EnumeratorCancellation] CancellationToken ct)
        //{
        //    byte[] buffer = new byte[_bufferSize];
        //    var checkNoise = false;
        //    var noiseBuffer = -1;


        //    while (ct.IsCancellationRequested is false)
        //    {
        //        int received = 0;
        //        try
        //        {
        //            if (checkNoise)
        //            {
        //                buffer = new byte[noiseBuffer];
        //            }

        //            received = await connectionSocket.MyReceiveAsync(buffer, 0, ct);
        //            if (received == 0)
        //            {
        //                Console.WriteLine($"Сработал if в методе ReadDataAsync | Клиент {currentId} | Принято {received}");
        //                yield break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Сработал catch в методе ReadDataAsync | {ex.Message}");
        //        }

        //        if (TryParseData(buffer, received, currentId, ct, out double dataValue) && checkNoise)
        //        {
        //            yield return dataValue;
        //            checkNoise = false;
        //        }
        //        else
        //        {
        //            if (TryReadNoise(buffer, received, currentId, ct, out noiseBuffer))
        //            {
        //                yield return noiseBuffer;
        //                checkNoise = true;
        //            }
        //            // отмена операции
        //        }
        //    }
        //}

        //private bool TryParseData(byte[] buffer, int received, int currentId, CancellationToken ct, out double dataValue) // можно возвращать массив байт, если нужно будет переделывать на общий случай
        //{
        //    dataValue = -1;

        //    byte type1 = (byte)DataTypes.first; // 0

        //    if (received == 0)
        //    {
        //        Console.WriteLine($"Сработал метод TryParseData | Клиент {currentId} | Ошибка приема {received} переданных байт");
        //        return false; // вот тут надо сделать отмену операции
        //    }

        //    var size = BitConverter.ToUInt32(buffer.AsSpan(0, _intSizeOfBytesArray));

        //    if (size == _bufferSize && buffer[4] == type1)
        //    {
        //        dataValue = BitConverter.ToDouble(buffer, 5);
        //        Console.WriteLine($"Сработал метод TryParseData | Клиент {currentId} | Принято {size} байт | Полученное значение {dataValue}");
        //        return true;
        //    }
        //    else 
        //    {
        //        Console.WriteLine($"Сработал метод TryParseData | Шум Клиент {currentId} | Размер шума {size}"); //  | Делаю буфер длиной {size - buffer.Length}
        //        return false;
        //    }
        //}

        //private bool TryReadNoise(byte[] inputBuffer, int totalReceived, int currentId, CancellationToken ct, out int noiseBufferSize)
        //{
        //    noiseBufferSize = -1;
        //    byte type2 = (byte)DataTypes.second; // 1

        //    var size = BitConverter.ToUInt32(inputBuffer.AsSpan(0, _intSizeOfBytesArray));

        //    if (inputBuffer.Length < _bufferSize && size == inputBuffer[4])
        //    {
        //        return true;
        //    }
        //    else if (inputBuffer.Length > _bufferSize && size == inputBuffer[4])
        //    {
        //        noiseBufferSize = (int)size - (int)_bufferSize;
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}




