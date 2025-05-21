using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.connections;

internal class Listener
{
    private Socket _listeningSocket;

    public static Listener StartListening() 
    { 
        throw new NotImplementedException();
    }

    public ChannelSender WaitChannelRequest()
    {
        throw new NotImplementedException();
    }

}

internal class ChannelSender
{
    private Socket _clientSocket;

    public Task SendTypeOn(double value)
    {
        throw new NotImplementedException();
    }

    public Task SendTypeTwo(byte[] data)
    {
        throw new NotImplementedException();
    }

}
