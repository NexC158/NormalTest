using SignalSource.events;

using System.Collections.Concurrent;
using SignalSource.connections;


namespace SignalSource.channels;

delegate Task ChannelEventHandler();

internal class ClientsManager
{
    private int _currentChannel = 0;

    private ConcurrentDictionary<int, ChannelManager> _channelsDict = new ConcurrentDictionary<int, ChannelManager>();

    public async Task ManageRequests()
    {
        var sync = new EventSynchronizator(); // var sync = new EventSynchronizator();

        var eventsBuilder = new EventsBuilder();

        var listener = Listener.StartListening(10000); // listener = StartListening()
        do
        {
            try
            {
                var channelId = Interlocked.Increment(ref _currentChannel);

                var sender = await listener.WaitChannelRequest(channelId); // sender = listener.StartListening()
                                                                  // var eventsBuilder = new EventsBuilder()
                var channel = new ChannelManager(sender, eventsBuilder, sync); // channel = new (sender, sync, eventsBuilder)
                
                

                _channelsDict.TryAdd(channelId, channel); // store channel to dict
                                                             // simple: _= channel.ProccessChannel();
                _ = HandleChannelProcessing(channel, channelId);// normal : _= HandleChannelProcessing(channel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сработал catch в ManageRequests | {ex.Message}");
            }

        } while (true);
    }

    private async Task HandleChannelProcessing(ChannelManager channel, int channelId)
    {
        try
        {
            await channel.ProccessChannel(channelId);

            

            //channel.ChannelIsCreated();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            // вот тут сделать вызов метода для реконнекта, и уже в нем отменять подписки после какого-то числа безуспешных попыток реконнекта

            //_channelsDict.TryRemove(channelId, out channel);

            //await channel.UnsubscribeChannel();
        }
        //finally
        //{
        //    await Task.Delay(1000);
        //    await channel.UnsubscribeChannel();
        //_channelsDict.TryRemove(channelId, out channel);// remove channel from dict
        //}
    }
}
