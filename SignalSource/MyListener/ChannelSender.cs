using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.Listener
{
    internal class ChannelSender
    {
        private StartListenersAsync _startListenerAsync = new StartListenersAsync();
        private Socket _listenerSocket;

        public async Task<Socket> ServerStartListen()
        {
            _listenerSocket = await StartListenersAsync.StartListenAsync();
            return _listenerSocket;
        }

        /*public async Task<Socket> AcceptClientsAsync()
        {
            return _startListenerAsync.ServerStartListen(_listenerSocket);
        }*/
    }
}
