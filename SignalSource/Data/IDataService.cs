using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.Data
{
    public interface IDataService
    {
        double FirstTypeDataGeneration();
        double[] SecondTypeDataGeneration();
        Task DataSendAsync(Socket client, double rndDoubleValue);
    }
}
