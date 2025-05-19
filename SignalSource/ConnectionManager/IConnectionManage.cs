using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.ConnectionManager
{
    public interface IConnectionManage
    {
        Task<Socket> StartListenerAsync();
        Task<Socket> ClientsConnectionSocket(Socket connection);
    }
}
