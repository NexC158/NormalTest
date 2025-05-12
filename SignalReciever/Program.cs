using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalReciever
{
    public class Program // client
    {
        public static async void Main(string[] args)
        {
            var tcpEndPoint = new IPEndPoint(IPAddress.Any, 10000);

            var connections = new Task[100];

            for (int i = 0; i < connections.Length; i++)
            {
                connections[i] = Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            await socket.ConnectAsync(tcpEndPoint);

                            byte[] buffer = new byte[30]; // сделать протокол

                            var recievedBytes = await socket.ReceiveAsync(buffer);

                            var recievedData = BitConverter.GetBytes(recievedBytes);
                            Console.WriteLine($"Соединие # {i} получило сообщение {recievedData}");
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine($"Соединие # {i} ошибка {e.Message}");
                        }
                    }

                });
            }
            await Task.WhenAll(connections);
        }

        



    }
}
