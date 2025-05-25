using SignalSource._example_.events;

using SignalSource._example_.connections;

using System.Collections.Concurrent;


namespace SignalSource._example_.channels;

delegate Task ChannelEventHandler();

internal class ClientsManager
{
    private int _currentChannel = 0;

    private ConcurrentDictionary<int, ChannelManager> _channelsDict = new ConcurrentDictionary<int, ChannelManager>();

    public event ChannelEventHandler ChannelIsCreated;

    public async Task ManageRequests()
    {
        var sync = new EventSynchronizator(); // var sync = new EventSynchronizator();

        var listener = Listener.StartListening(); // listener = StartListening()
        do
        {
            // try

            var sender = listener.WaitChannelRequest(); // sender = listener.StartListening()

            var eventsBuilder = new EventsBuilder(); // var eventsBuilder = new EventsBuilder()

            var channel = new ChannelManager(sender, eventsBuilder, sync); // channel = new (sender, sync, eventsBuilder)

            var channelId = Interlocked.Increment(ref _currentChannel);

            _channelsDict.TryAdd(channelId, channel); // store channel to dict


            _ = Task.Run(()=> HandleChannelProcessing(channel, channelId));

            

            // catch

            // simple: _= channel.ProccessChannel();
            // normal : _= HandleChannelProcessing(channel);

        } while (true);
    }

    private async Task HandleChannelProcessing(ChannelManager channel, int channelId)
    {
        try
        {
            
            await channel.ProccessChannel();
            //ChannelIsCreated += ChannelManager.ProccessChannel;

            //channel.ChannelIsCreated();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            // вот тут сделать вызов метода для реконнекта, и уже в нем отменять подписки после какого-то числа безуспешных попыток реконнекта

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
