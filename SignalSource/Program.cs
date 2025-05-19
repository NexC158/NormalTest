using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SignalSource.Synchronization;
using SignalSource.ConnectionManager;
using SignalSource.Data;

namespace SignalSource
{
    internal class Program // server 
    { 
        public static async Task Main(string[] args)
        {
            var connect = new ConnectionManage();
            var data = new DataService();
            var sync = new ClientSynchronization(connect, data);
            
            await sync.StartTransferDataAsync();


            /*var tcpEndPoint = new IPEndPoint(IPAddress.Any, 10000);
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(tcpEndPoint);
            listener.Listen(100);
            Console.WriteLine($"Сервер запущен на {listener.LocalEndPoint}");

            while (true)
            {
                var client = await listener.AcceptAsync();
                Console.WriteLine($"Новый клиент: {client.RemoteEndPoint}");

                // Запускаем отдельный поток для клиента
                var tasksConnection = Task.Run(async () =>
                {
                    try
                    {
                        using (client)
                        {
                            var random = new Random();
                            while (true)
                            {
                                double value = random.NextDouble();
                                byte[] data = BitConverter.GetBytes(value);

                                // Гарантированная отправка
                                int totalSent = 0;
                                while (totalSent < data.Length) // totalSent < data.Length
                                {
                                    int sent = await client.SendAsync(
                                        new ArraySegment<byte>(data, totalSent, data.Length - totalSent)
                                    );
                                    if (sent == 0) throw new Exception("Соединение разорвано");
                                    totalSent += sent;
                                }

                                Console.WriteLine($"Отправлено клиенту {client.RemoteEndPoint}: {value}");
                                await Task.Delay(10);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка с клиентом: {ex.Message}");
                    }
                });
            }*/
        }
    }
}