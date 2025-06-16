using System.Net.Sockets;

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

    public bool IsConnected()
    {
        return _connectionToServerSocket.Connected;
    }
}
