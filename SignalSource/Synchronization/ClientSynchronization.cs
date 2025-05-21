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
        private int _maxConnections = 0;
        private readonly CancellationTokenSource _ct = new();

        public ClientSynchronization(IConnectionManage listener, IDataService dataService)
        {
            _listener = listener;
            _dataService = dataService;
        }

        public async Task StartTransferDataAsync()
        {
            var listener = await _listener.StartListenerAsync();

            

            try
            {
                do
                {

                    var client = await _listener.ClientsConnectionSocket(listener);


                    //var clientId = Interlocked.Increment(ref _countConnections);
                    _clients.TryAdd(Interlocked.Increment(
                        ref _countConnections),
                        client);
                        // _dataService.DataSendAsync(client, _dataService.FirstTypeDataGeneration()) // а это наверное буду использовать там,  
                           // где открою этот словарь для отправки значений
                    Console.WriteLine($"К серверу подключен клиент {_countConnections}");
                } while (!_ct.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Task<Socket> ProcessNewClinet(Socket client)
        {
            return  _listener.ClientsConnectionSocket(client);
        }
    }
}
