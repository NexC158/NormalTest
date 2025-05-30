using SignalSource.connections;
using SignalSource.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.channels;

internal class ChannelManager
{
    private readonly ChannelSender _sender;
    private readonly EventsBuilder _eventsBuilder;
    private readonly EventSynchronizator _sync;
    private object _syncLock = new object();
    private int currnentConnect = 0;
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
            await _sender.SendTypeOne(_eventsBuilder.BuildTypeOne());
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
            await _sender.SendTypeTwo(_eventsBuilder.BuildTypeTwo());
            Console.WriteLine("Сработал SendingDataTypeTwo | Отправлен второй тип");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task ProccessChannel(int channelId)
    {
        _sync.TimeToSendTypeOne += async () => await SendingDataTypeOne();

        _sync.TimeToSendTypeTwo += async () => await SendingDataTypeTwo();

        _sync.StartTimers();


        Console.WriteLine($"Сработал ProccessChannel, подписка на ивенты | текущее количество подпищеков {channelId}");



    }

    //public async Task UnsubscribeChannel()
    //{
    //    lock (_syncLock)
    //    {
    //        _sync.TimeToSendTypeOne -= async () => await SendingDataTypeOne();
    //        _sync.TimeToSendTypeTwo -= async () => await SendingDataTypeTwo();
    //        _sync.StopTimers();
    //    }
    //}
}
