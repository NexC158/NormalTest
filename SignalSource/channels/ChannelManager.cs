using SignalSource.connections;
using SignalSource.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalSource.channels;

internal class ChannelManager
{
    private readonly ChannelSender _sender;

    private readonly EventsBuilder _eventsBuilder;

    private readonly EventSynchronizer _sync;

    private readonly Random _random = new Random();
    public ChannelManager(ChannelSender sender, EventsBuilder eventsBuilder, EventSynchronizer sync)
    {
        _sender = sender;
        _eventsBuilder = eventsBuilder;
        _sync = sync;

    }

    public async Task ProccessChannel2(int channelId) 
    {
        Func<Task> handler1 = async () =>                    
        {                                             
            await this._sender.SendTypeOne(_eventsBuilder.BuildTypeOne());
        };

        this._sync.GlobalTimerType1 += handler1;
        try
        {
            while (this._sender.IsConnected()) // сюда хочу ct
            {
                int timeForNextEvetn2 = _random.Next(400, 600); 
                await Task.Delay(timeForNextEvetn2);

                await this._sender.SendTypeTwo(_eventsBuilder.BuildTypeTwo());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch в методе ProccessChannel2 | {ex.Message}");
        }
        finally 
        {
            this._sync.GlobalTimerType1 -= handler1;
        }
    }
}
