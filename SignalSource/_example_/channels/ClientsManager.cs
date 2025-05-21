using SignalSource._example_.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalSource._example_.channels;
internal class ClientsManager
{
    public async Task ManageRequests()
    {
        // var sync = new EventSyncronizator();

        // listener = StartListening()
        do
        {
            // sender = listener.StartListening()
            // var eventsBuilder = new EventsBuilder()
            // channel = new (sender, sync, eventsBuilder)
            // store channel to dict

            // simple: _= channle.ProccessChannel();
            // normal : _= HandleChannelProcessing(channel);

        } while (true);
    }

    private async Task HandleChammelProcessing(ChannelManager channel)
    {
        await channel.ProccessChannel();
        // remove channel from dict
    }
}
