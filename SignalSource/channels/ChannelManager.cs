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

    public async Task ProccessChannel2(int channelId) // тут надо сделать отслеживание отправки данных
    {                                                 // типа я кладу хэндлер в очередь а в другом потоке я считаываю очередь и отправляю // Но очередь будет биться, если у меня отвалится какой-то сокет
        Func<Task> handler1 = async () =>       // Обработчик события                             // так же мне надо синхронизировать отправку, чтобы одно не мешало другому
        {                                             // , который вызывает SendTypeOne через _sender
            await this._sender.SendTypeOne(_eventsBuilder.BuildTypeOne());
        };

        this._sync.GlobalTimerType1 += handler1; // Обработчик события handler1 подписывается на событие GlobalTimerType1 через объект _sync

        await _sync.ProcessAsyncEvent(handler1);

        try
        {
            while (this._sender.IsConnected()) // сюда хочу cts чтобы он существовал в отдельном потоке и срабатывал только когда что-то отвалилось 
            {
                int timeForNextEvetn2 = _random.Next(400, 600); // вот тут посчитать как реализовать плотность сигнала

                await Task.Delay(timeForNextEvetn2);

                await this._sender.SendTypeTwo(_eventsBuilder.BuildTypeTwo());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch в методе ProccessChannel2 | {ex.Message}");
        }
        finally // у меня неправильно построен блок трай файналли, 
        {
            if (this._sender.IsConnected() is false) this._sync.GlobalTimerType1 -= handler1;

        }
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
