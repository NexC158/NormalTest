using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource
{
    public class Program // server 
    {
        public static async void Main(string[] args)
        {
            var TcpEndPoint = new IPEndPoint(IPAddress.Any, 10000);

            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(TcpEndPoint);

            Console.WriteLine($"Сервер начинает слушать порт: {socket.LocalEndPoint}");

            socket.Listen(100);

            do
            {
                var connectedConnection = await socket.AcceptAsync();
                Console.WriteLine($"Новое подключение {connectedConnection.RemoteEndPoint}");

                var connectionTask = Task.Run(async () =>
                {
                    try
                    {
                        using (connectedConnection)
                        {
                            var random = new Random();

                            double randomDoubleData = random.NextDouble();
                            byte[] bytesToSend = BitConverter.GetBytes(randomDoubleData);

                            var sentData = await connectedConnection.SendAsync(bytesToSend);
                            await Task.Delay(1000); // сделать синхронизацию с клиентом
                        }
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine($"Ошибка {e.Message}");
                    }

                });

            } while (socket.Connected);
        }

        public static async Task PeriodPeredachi(Task task)
        {
            await task;
            await Task.Delay(1000);
        }

        public static async Task Peredacha(Socket socket)
        {
            using var connection = await CreateNewConnectionAsync(socket);
            await SendDataAsync(connection);
        }


        public static async Task<Socket> CreateNewConnectionAsync(Socket socket)
        {
            using Socket connection = await socket.AcceptAsync();
            return connection;
        }

        public static async Task SendDataAsync(Socket socket)
        {
            var dataToSend = GenerateMessage();
            await socket.SendAsync(dataToSend);
        }

        public static byte[] GenerateMessage()
        {
            var randomDouble = RandomGeneration();

            return BitConverter.GetBytes(randomDouble);
        }

        public static double RandomGeneration()
        {
            var random = new Random();
            var result = random.NextDouble();
            return result;
        }
    }
}