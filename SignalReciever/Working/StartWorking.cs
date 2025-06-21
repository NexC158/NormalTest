using System.Collections.Concurrent;
using System.Net.Sockets;
using SignalRecieverAnalyzer.Connection;
using SignalRecieverAnalyzer.Data;


namespace SignalRecieverAnalyzer.Working
{
    internal class StartWorking
    {
        private ConcurrentDictionary<int, Task> _connectionTasks = new ConcurrentDictionary<int, Task>();

        private int _currentConnection = 0;

        private readonly CancellationTokenSource _ct = new(); // TODO сделать локальный ct для точечной отмены

        private readonly DataAnalyze dataAnalyzer = new DataAnalyze();

        public async Task StartConnectingToServer(int countConnections)
        {
            try
            {
                for (int i = 0; i < countConnections; i++)
                {
                    var connectedId = Interlocked.Increment(ref _currentConnection);

                    Console.WriteLine($"Сработал StartConnectingToServer | Начинаю попытку подключения для {connectedId} клиента");

                    var newTaskConnection = WorkingWithConnection(connectedId, _ct.Token);

                    _connectionTasks.TryAdd(connectedId, newTaskConnection);
                }
                await Task.WhenAll(_connectionTasks.Values);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StartConnectingToServer | {ex.Message}");
            }
        }

        private async Task WorkingWithConnection(int connectedId, CancellationToken ct)
        {
            var dataFilter = new DataFilter();
           
            ClientConnection connection = null;

            while (connection == null)
            {
                try
                {
                    connection = await ClientConnection.ConnectionToServerAsync("127.0.0.1", 10000, ct);
                    Console.WriteLine($"Сработал WorkingWithConnection | Подключен клиент: {connectedId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch WorkingWithConnection | Попытка подключения для клиента {connectedId} провелена, пробую заново | {ex.Message}");
                }
            }

            while (ct.IsCancellationRequested is false)
            {
                try
                {
                    await foreach (var data in dataFilter.DataFilterAsync(connection, connectedId, ct))// данные с этого foreach должен брать аналайзер
                    {
                        await dataAnalyzer.WriteToChannelAsync(data, ct);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch WorkingWithConnection | Ошибка подключения, клиент {connectedId} {ex.Message} ");
                }
                finally
                {
                    connection.SocketDisconnect();

                    try
                    {
                        connection = await ClientConnection.ConnectionToServerAsync("127.0.0.1", 10000, ct);
                        ResetConnection(connectedId, ct);
                        Console.WriteLine($"Сработал finally в WorkingWithConnection | клиент {connectedId} переподключен");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Что-то не так | cработал catch в finally в WorkingWithConnection |{ex.Message}");
                    }
                    
                }
            }
        }

        public void ResetConnection(int badConnectedId, CancellationToken ct)
        {
            try
            {
                var newTaskConnectoin = WorkingWithConnection(badConnectedId, ct);
                // if (_connectionTasks.TryGetValue(connectedId, out Task oldTask))
                _connectionTasks.TryUpdate(badConnectedId, newTaskConnectoin, _connectionTasks[badConnectedId]);
                // else _connectionTasks.TryAdd(connectedId, newTaskConnectoin);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в методе Disconnect | {ex.Message}");
            }
        }

        public async Task StopAllOperations()
        {
            _ct.Cancel();

            try
            {
                await Task.WhenAll(_connectionTasks.Values);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отмены задач | {ex.Message}");
            }
        }
    }
}
