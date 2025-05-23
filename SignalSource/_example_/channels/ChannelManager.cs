using SignalSource._example_.connections;
using SignalSource._example_.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.channels;

internal class ChannelManager // это подпищик
{
    private readonly ChannelSender _sender;
    private readonly EventsBuilder _eventsBuilder;
    private readonly EventSynchronizator _sync;

    public ChannelManager(ChannelSender sender, EventsBuilder eventsBuilder, EventSynchronizator sync)
    {
        _sender = sender;
        _eventsBuilder = eventsBuilder;
        _sync = sync;

    }

    private async void SendDataTypeOne()
    {
        try
        {
            var buildData = _eventsBuilder.BuildTypeOne();
            await _sender.SendTypeOne(buildData);
            Console.WriteLine("отправлен первый тип");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void SendDataTypeTwo()
    {
        try
        {
            var buildData = _eventsBuilder.BuildTypeTwo();
            await _sender.SendTypeTwo(buildData);
            Console.WriteLine("отправлен второй тип");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task ProccessChannel()
    {
        
        _sync.TimeToSendTypeOne += SendDataTypeOne;
        _sync.TimeToSendTypeTwo += SendDataTypeTwo;
        _sync.NotificateOfNewConnection();
    }

    public async Task UnsubscribeChannel()
    {
        _sync.TimeToSendTypeOne -= SendDataTypeOne;
        _sync.TimeToSendTypeTwo -= SendDataTypeTwo;
        _sync.UnsubscribeConnection();
    }
}
