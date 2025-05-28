using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalSource._example_.channels;

namespace SignalSource._example_.connections;

internal class Listener
{
    private Socket _listeningSocket;

    private Listener(Socket listeningSocket)
    {
        _listeningSocket = listeningSocket;
    }

    public static Listener StartListening(int port) 
    {
        var listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));

        listeningSocket.Listen(100);

        Console.WriteLine($"Начинаю слушать порт: {port} и ip: {IPAddress.Any}");

        return new Listener(listeningSocket);
    }

    public async Task<ChannelSender> WaitChannelRequest()
    {
        var clientSocketAccept = await _listeningSocket.AcceptAsync();

        Console.WriteLine("Получено новое подключение. Сработал метод WaitChannelRequest()");
        
        return new ChannelSender(clientSocketAccept);
    }

}


