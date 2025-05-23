using SignalSource._example_.events;

using SignalSource._example_.connections;

using System.Collections.Concurrent;


namespace SignalSource._example_.channels;
internal class ClientsManager
{
    private int _currentChannel = 0;

    private ConcurrentDictionary<int, ChannelManager> _channelsDict = new ConcurrentDictionary<int, ChannelManager>();

    public async Task ManageRequests()
    {
        var sync = new EventSynchronizator(); // var sync = new EventSynchronizator();

        var listener = connections.Listener.StartListening(); // listener = StartListening()
        do
        {
            // try
            var sender = await listener.WaitChannelRequest(); // sender = listener.StartListening()

            var eventsBuilder = new EventsBuilder(); // var eventsBuilder = new EventsBuilder()

            var channel = new ChannelManager(sender, eventsBuilder, sync); // channel = new (sender, sync, eventsBuilder)

            _channelsDict.TryAdd(Interlocked.Increment(ref _currentChannel), channel); // store channel to dict

            _ = Task.Run(()=> HandleChannelProcessing(channel));
            // catch

            // simple: _= channel.ProccessChannel();
            // normal : _= HandleChannelProcessing(channel);

        } while (true);
    }

    private async Task HandleChannelProcessing(ChannelManager channel)
    {
        try
        {
            await channel.ProccessChannel();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await Task.Delay(1000);
            await channel.UnsubscribeChannel();
        }
        
        //_channelsDict.TryRemove(channelId, out channel);// remove channel from dict
    }
}
