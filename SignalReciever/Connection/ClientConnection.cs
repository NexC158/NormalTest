using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRecieverAnalyzer.Connection
{
    public class ClientConnection : IClientConnection
    {
        public async Task<Socket> ConnectionAsync()
        {
			var ip = "127.0.0.1";
			var port = 10000;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(ip, port);
            return socket;
        }
    }
}
