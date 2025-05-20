using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.ConnectionManager
{
    public class ConnectionManage : IConnectionManage
    {
        public async Task<Socket> StartListenerAsync()
        {
            var port = 10000;
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(100);
            Console.WriteLine($"Начинаю слушать порт: {port} и ip: {IPAddress.Any}");
            return listener;
        }

        public async Task<Socket> ClientsConnectionSocket(Socket listener)
        {
            var connection = await listener.AcceptAsync();

            return connection;
        }

        
    }
}
