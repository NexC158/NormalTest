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
    private object _syncLock = new object();

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
            Console.WriteLine("Сработал SendingDataTypeOne | Отправлен первый тип");
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
            Console.WriteLine("Сработал SendingDataTypeTwo | Отправлен второй тип");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task ProccessChannel()
    {
        var currnentConnect = 1;
        lock (_syncLock)
        {

            _sync.TimeToSendTypeOne += async () => await SendingDataTypeOne();
            _sync.TimeToSendTypeTwo += async () => await SendingDataTypeTwo();

            _sync.StartTimers();

            Console.WriteLine($"Сработал ProccessChannel, подписка на ивенты | текущее количество подпищеков {currnentConnect}");
            currnentConnect++;
        }
    }

    public async Task UnsubscribeChannel()
    {
        lock (_syncLock)
        {
            _sync.TimeToSendTypeOne -= async () => await SendingDataTypeOne();
            _sync.TimeToSendTypeTwo -= async () => await SendingDataTypeTwo();
            _sync.StopTimers();
        }
    }
}
