using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalSource.events;

namespace SignalSource.channels;

internal class ChannelSender
{
    private readonly Socket _clientSocket;

    
    private EventsBuilder _eventsBuilder;
    public ChannelSender(Socket clientSocket)
    {
        _clientSocket = clientSocket;
        _eventsBuilder = new EventsBuilder();
    }

    public async Task SendTypeOne()
    {
        var dataToSend = _eventsBuilder.BuildTypeOne();
        Console.WriteLine($"Отправлен первый тип | {BitConverter.ToDouble(dataToSend, 5)}");
        await _clientSocket.SendAsync(dataToSend);
    }

    public async Task SendTypeTwo()
    {
        var dataToSend = _eventsBuilder.BuildTypeTwo();
        Console.WriteLine($"Отправлен второй тип | {BitConverter.ToUInt32(dataToSend, 0)}");
        await _clientSocket.SendAsync(dataToSend);
    }
}

