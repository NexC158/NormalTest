using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SignalRecieverAnalyzer.Connection;


internal class ClientConnection
{
    public Socket _connectionToServerSocket { get; private set; }

    public ClientConnection(Socket connectionToServerSocket)
    {
        _connectionToServerSocket = connectionToServerSocket;
    }

    public static async Task<ClientConnection> ConnectionToServerAsync(string ip, int port)
    {
        var connectionSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        await connectionSoket.ConnectAsync(ip, port);

        return new ClientConnection(connectionSoket);
    }
}
