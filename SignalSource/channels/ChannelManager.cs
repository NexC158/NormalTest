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

    private readonly EventSynchronizer _sync;


    private int currnentConnect = 0;

    private readonly Random _random;
    public ChannelManager(ChannelSender sender, EventsBuilder eventsBuilder, EventSynchronizer sync)
    {
        _sender = sender;
        _eventsBuilder = eventsBuilder;
        _sync = sync;

    }

    public async Task ProccessChannel2( int channelId)
    {
        EventHandler handler1 = async (s, e) =>
        {
            _ = this._sender.SendTypeOne();
        };

        this._sync.GlobalTimerType1 += handler1;

        while (_sender.IsConnectd())
        {
            int timeForNextEvetn2 = _random.Next(1, 1000);
            await Task.Delay(timeForNextEvetn2);
            await this._sender.SendTypeTwo();
        }

        this._sync.GlobalTimerType1 -= handler1;
    }

#if false
    public async Task ProccessChannel(ChannelManager channel, int channelId)
    {
        // Вот тут я должен сделать таймеры и через них инвокать события. А события должны запускать методы из класса ChannelSender 

        

        var personalTimerForType2 = new System.Timers.Timer(500);

        globalTimerForType1.Elapsed += (s, e) =>
        {
            channel._sync.OnGlobalTimerType1(s, e);
        };

        personalTimerForType2.Elapsed += (s, e) =>
        {
            channel._sync.OnPersonalTimerType2(s, e);
        };

        EventHandler handler1 = async (s, e) =>
        {
            _ = channel._sender.SendTypeOne();
        };

        channel._sync.GlobalTimerType1 += handler1;

        EventHandler handler2 = (s, e) =>
        {
            _ = channel._sender.SendTypeTwo();
        };

        channel._sync.PersonalTimerType2 += handler2;


        // wait disconnets

        // finally:
        //  -= handler1
        // -= handler2

        // globalTimerForType1.Start();

        personalTimerForType2.Start();
    }
#endif
}
