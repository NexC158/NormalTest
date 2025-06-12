using SignalSource.events;

using System.Collections.Concurrent;
using SignalSource.connections;


namespace SignalSource.channels;

internal class ClientsManager
{
    private int _currentChannel = 0;

    private ConcurrentDictionary<int, ChannelManager> _channelsDict = new ConcurrentDictionary<int, ChannelManager>();

    public async Task ManageRequests()
    {
        using var sync = new EventSynchronizer();

        var eventsBuilder = new EventsBuilder();

        var listener = Listener.StartListening(10000);
        do
        {
            try
            {
                var channelId = Interlocked.Increment(ref _currentChannel);

                var sender = await listener.WaitChannelRequest(channelId);

                sync.StartIfNotYet();

                var channel = new ChannelManager(sender, eventsBuilder, sync);

                _channelsDict.TryAdd(channelId, channel);


                _ = HandleChannelProcessing(channel, channelId);
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
            await channel.ProccessChannel2(channelId);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch в HandleChannelProcessing | {ex.Message}");

            // вот тут сделать ожидание для реконнекта, и уже в нем отменять подписки после какого-то числа безуспешных попыток реконнекта

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

