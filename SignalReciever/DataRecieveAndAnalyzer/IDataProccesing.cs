using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalRecieverAnalyzer.DataRecieveAndAnalyzer
{
    internal interface IDataProccesing
    {
        Task<double> RecieveDoubleAsync(Socket socket);
        Task<byte[]> Analyzer(Socket socket1, Socket socket2);
        
    }
}
