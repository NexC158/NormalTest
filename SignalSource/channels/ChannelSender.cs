using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.channels
{
    internal class ChannelSender
    {
        private Socket _clientSocket;

        public ChannelSender(Socket clientSocket)
        {
            _clientSocket = clientSocket;
        }

        public async Task SendTypeOne(byte[] data1)
        {
            await _clientSocket.SendAsync(data1);
        }

        public async Task SendTypeTwo(byte[] data2)
        {
            await _clientSocket.SendAsync(data2);
        }

    }
}
