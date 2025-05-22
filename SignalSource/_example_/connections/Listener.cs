using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.connections;

internal class Listener
{
    private Socket _listeningSocket;

    private Listener(Socket listeningSocket)
    {
        _listeningSocket = listeningSocket;
    }

    public static Listener StartListening() 
    {
        var port = 10000;

        var listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));

        listeningSocket.Listen(100);

        Console.WriteLine($"Начинаю слушать порт: {port} и ip: {IPAddress.Any}");

        return new Listener(listeningSocket);
    }

    public async Task<ChannelSender> WaitChannelRequest()
    {
        var clientSocketAccept = await _listeningSocket.AcceptAsync();

        Console.WriteLine("Получено новое подключение");
        
        return new ChannelSender(clientSocketAccept);
    }

}

internal class ChannelSender
{
    private Socket _clientSocket;

    public ChannelSender(Socket clientSocket)
    {
        _clientSocket = clientSocket;
    }

    enum DataTypes : byte // да я знаю что он по умолчанию ставится как int, написал для себя, чтобы когда у меня не будет что-то работать, я сразу обратил внимание что это не из-за enama 
    {
        first = 1,
        second = 2
    }

    public async Task SendTypeOne(double value) // что-то не то
    {
        var dataToSend = BitConverter.GetBytes(value);
        await SendDataAsync(DataTypes.first, dataToSend);
    }

    public async Task SendTypeTwo(byte[] data)
    {
        await SendDataAsync(DataTypes.first, data);
    }

    private async Task SendDataAsync(DataTypes dataType, byte[] dataToSend)
    {
        try
        {
            var typeBytes = BitConverter.GetBytes((int)dataType);

            var sizeBytes = BitConverter.GetBytes(sizeof(uint) + dataToSend.Length);

            var formingBytesArray = new byte[sizeBytes.Length + typeBytes.Length + dataToSend.Length];

            Buffer.BlockCopy(sizeBytes, 0, formingBytesArray, 0, sizeBytes.Length); // мб использовать MemoryStream
            Buffer.BlockCopy(typeBytes, 0, formingBytesArray, sizeBytes.Length, typeBytes.Length);
            Buffer.BlockCopy(dataToSend, 0, formingBytesArray, sizeBytes.Length + typeBytes.Length, dataToSend.Length);

            await _clientSocket.SendAsync(formingBytesArray);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}
