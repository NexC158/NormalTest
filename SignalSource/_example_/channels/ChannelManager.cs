using SignalSource._example_.connections;
using SignalSource._example_.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.channels;

internal class ChannelManager
{
    private readonly ChannelSender _sender;
    private readonly EventsBuilder _eventsBuilder;
    private readonly EventSynchronizator _sync;

    private Random _random = new Random();

    public ChannelManager(ChannelSender sender, EventsBuilder eventsBuilder, EventSynchronizator sync)
    {
        _sender = sender;
        _eventsBuilder = eventsBuilder;
        _sync = sync;

    }

    private async Task SendingDataTypeOne()
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

    private async Task SendingDataTypeTwo()
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
        _sync.TimeToSendTypeOne += async () => await SendingDataTypeOne();
        _sync.TimeToSendTypeTwo += async () => await SendingDataTypeTwo();

        _sync.StartTimers();
    }

    public async Task UnsubscribeChannel()
    {

    }
}
