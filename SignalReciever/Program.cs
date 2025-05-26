using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalRecieverAnalyzer.Connection;
using SignalRecieverAnalyzer.DataRecieveAndAnalyzer;
using SignalRecieverAnalyzer.Working;


namespace SignalReciever
{
    public class Program // client
    {
        public static async Task Main(string[] args)
        {
            var sync = new StartWorking();

            await sync.ConnectingToServerAsync(100);

        }
    }
}
