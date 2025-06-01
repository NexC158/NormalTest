using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SignalSource.channels;

namespace SignalSource
{
    public class Program // server 
    { 
        public static async Task Main(string[] args)
        {
            var manager = new ClientsManager();

             await manager.ManageRequests();
            

        }
    }
}

// 1) это класс снаружи моих сокетов внутри держит сокет. он прячет все. Channel Sender




// в тайпе 1 байт 


// big-endian и little-endian  // делать по машинному