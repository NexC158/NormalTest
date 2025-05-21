using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalRecieverAnalyzer.Connection;
using SignalRecieverAnalyzer.DataRecieveAndAnalyzer;
using SignalRecieverAnalyzer.Working;


namespace SignalReciever
{
    public class Program // client
    {
        public static async Task Main(string[] args)
        {
            var connector = new ConnectionToServer();
            var recieve = new DataProccesing();
            var sync = new StartWorking();

            await sync.StartRecieveAsync();
            
            /*var countOfConnections = 100;
            var tasks = new Task[countOfConnections];

            for (int i = 0; i < countOfConnections; i++)
            {
                int clientId = i;
                tasks[i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            Console.WriteLine($"Клиент {clientId} подключается...");
                            await socket.ConnectAsync("127.0.0.1", 10000);
                            Console.WriteLine($"Клиент {clientId} подключен");

                            byte[] buffer = new byte[8];
                            while (true)
                            {
                                // Гарантированное чтение 8 байт (размер double)
                                int totalReceived = 0;
                                while (totalReceived < 8)
                                {
                                    int received = await socket.ReceiveAsync(
                                        new ArraySegment<byte>(buffer, totalReceived, 8 - totalReceived),
                                        SocketFlags.None
                                    );
                                    if (received == 0) throw new Exception("Сервер закрыл соединение");
                                    totalReceived += received;
                                }

                                double value = BitConverter.ToDouble(buffer, 0);
                                Console.WriteLine($"Клиент {clientId} получил: {value}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Клиент {clientId} ошибка: {ex.Message}");
                            await Task.Delay(2000); // Пауза перед переподключением
                        }
                    }
                });
            }
            await Task.WhenAll(tasks);*/
        }
    }
}
