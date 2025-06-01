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


    private int currnentConnect = 0;

    private readonly Random _random;
    public ChannelManager(ChannelSender sender, EventsBuilder eventsBuilder, EventSynchronizator sync)
    {
        _sender = sender;
        _eventsBuilder = eventsBuilder;
        _sync = sync;

    }

    public async Task ProccessChannel(ChannelManager channel, int channelId)
    {
        // Вот тут я должен сделать таймеры и через них инвокать события. А события должны запускать методы из класса ChannelSender 

        var globalTimerForType1 = new System.Timers.Timer(1000);

        var personalTimerForType2 = new System.Timers.Timer(500);

        globalTimerForType1.Elapsed += (s, e) =>
        {
            channel._sync.OnGlobalTimerType1(s, e);
        };

        personalTimerForType2.Elapsed += (s, e) =>
        {
            channel._sync.OnPersonalTimerType2(s, e);
        };

        channel._sync.GlobalTimerType1 += (s, e) =>
        {
            channel._sender.SendTypeOne();
        };

        channel._sync.GlobalTimerType1 -= (s, e) =>
        {
            channel._sender.SendTypeTwo();
        };

        globalTimerForType1.Start();
        personalTimerForType2.Start();
    }
}
