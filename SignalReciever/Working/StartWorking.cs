using System.Collections.Concurrent;
using System.Net.Sockets;
using SignalRecieverAnalyzer.Connection;
using SignalRecieverAnalyzer.Data;


namespace SignalRecieverAnalyzer.Working
{
    internal class StartWorking
    {
        private readonly ConcurrentDictionary<int, Task> _connectionTasks = new();

        private int _currentConnection = 0;

        private readonly CancellationTokenSource _ct = new(); // TODO сделать локальный ct для точечной отмены

        private readonly DataAnalyze dataAnalyzer = new ();

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

            connection = await CreateConnection(connection, connectedId, ct);

            while (ct.IsCancellationRequested is false)
            {
                try
                {
                    await foreach (var data in dataFilter.DataFilterAsync(connection, connectedId, ct))
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
                    connection = await CreateConnection(connection, connectedId, ct);

                    ResetConnection(connection, connectedId, ct);
                    Console.WriteLine($"Сработал finally в WorkingWithConnection | клиент {connectedId} переподключен");
                }
            }
        }

        private async Task<ClientConnection> CreateConnection(ClientConnection connection, int connectedId, CancellationToken ct)
        {
            while (connection == null)
            {
                try
                {
                    connection = await ClientConnection.ConnectionToServerAsync("127.0.0.1", 10000, ct);
                    Console.WriteLine($"Сработал WorkingWithConnection | Подключен клиент: {connectedId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch WorkingWithConnection | Попытка подключения для клиента {connectedId} провелена, пробую заново через секунду | {ex.Message}");
                    await Task.Delay(1000, ct);
                }
            }

            return connection;
        }

        public void ResetConnection(ClientConnection connection, int badConnectedId, CancellationToken ct)
        {
            try
            {
                var newTaskConnectoin = WorkingWithConnection(badConnectedId, ct); // проблема в этом
                _connectionTasks.TryUpdate(badConnectedId, newTaskConnectoin, _connectionTasks[badConnectedId]);
                // возможно стоит класть в словарь не таску WorkingWithConnection, а как раз await foreach (var data in dataFilter.DataFilterAsync(connection, connectedId, ct))
                // чтобы было проще разделять
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
