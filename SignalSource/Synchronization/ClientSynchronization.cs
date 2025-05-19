using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalSource.ConnectionManager;
using SignalSource.Data;

namespace SignalSource.Synchronization
{
    public class ClientSynchronization : IClientSynchronization
    {
        private readonly IConnectionManage _listener;
        private readonly IDataService _dataService;
        private ConcurrentDictionary<int, Task> _clients = new ConcurrentDictionary<int, Task>();
        private int _countConnections = 0;
        private readonly CancellationTokenSource _ct = new();

        public ClientSynchronization(IConnectionManage listener, IDataService dataService)
        {
            _listener = listener;
            _dataService = dataService;
        }

        public async Task StartTransferDataAsync()
        {
            var listener = await _listener.StartListenerAsync();
            var client = await _listener.ClientsConnectionSocket(listener);
            try
            {
                do
                {
                    var clientId = Interlocked.Increment(ref _countConnections);
                    _clients.TryAdd(clientId, Task.Run (()=>
                    _listener.ClientsConnectionSocket(listener) // _dataService.DataSendAsync(client, _dataService.FirstTypeDataGeneration())
                    ));                   
                    Console.WriteLine($"К серверу подключен клиент {_countConnections}");
                } while (!_ct.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
