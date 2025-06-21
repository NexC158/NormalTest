using System.Net.Sockets;

namespace SignalRecieverAnalyzer.Connection;
internal class ClientConnection
{
    public Socket _connectionToServerSocket { get; private set; }

    public ClientConnection(Socket connectionToServerSocket)
    {
        _connectionToServerSocket = connectionToServerSocket;
    }

    public static async Task<ClientConnection> ConnectionToServerAsync(string ip, int port, CancellationToken ct) // TODO
    {
        var connectionSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        await connectionSoket.ConnectAsync(ip, port, ct);

        return new ClientConnection(connectionSoket);
    }

    public bool IsConnected()
    {
        return _connectionToServerSocket.Connected;
    }

    public void SocketDisconnect()
    {
        if (_connectionToServerSocket == null)
        {
            return;
        }

        try
        {
            if (IsConnected())
            {
                _connectionToServerSocket.Shutdown(SocketShutdown.Both);
            }
        }
        finally
        {
            _connectionToServerSocket.Close();
        }
    }

    public async Task<int> MyReceiveAsync(byte[] value, int count, CancellationToken ct)
    {
        var res = await _connectionToServerSocket.ReceiveAsync(value, (SocketFlags)count, ct);
        return res;
    }

}
