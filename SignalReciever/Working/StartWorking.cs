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

        private readonly CancellationTokenSource _ct = new();

        public async Task StartConnectingToServer(int maxConnections)
        {
            try
            {
                for (int i = 0; i < maxConnections; i++)
                {
                    var connectedId = Interlocked.Increment(ref _currentConnection);

                    Console.WriteLine($"Сработал StartConnectingToServer | Начинаю попытку подключения для {connectedId} клиента");

                    _connectionTasks.TryAdd(connectedId, WorkingWithConnection(connectedId, _ct.Token));
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
            var data = new DataFilter();

            var connection = await ClientConnection.ConnectionToServerAsync("127.0.0.1", 10000);


            Console.WriteLine($"Сработал WorkingWithConnection | Подключен клиент: {connectedId}");

            try
            {
                while (true)
                {
                    await data.DataFilterAsync(connection, connectedId);
                    //Console.WriteLine($"неВечный цикл | WorkingWithConnection | Клиент {connectedId} получил данные: {recievedData}");

                    /*var recievedData =*/
                    await data.DataFilterAsync(connection, connectedId);

                    if (ct.IsCancellationRequested) break;

                    //Console.WriteLine($"Вечный цикл | WorkingWithConnection | Клиент {connectedId} получил данные: {recievedData}"); // выдает что recievedData == -1, не знаю почему

                    /*var recievedData =*/
                    await data.DataFilterAsync(connection, connectedId);

                    if (ct.IsCancellationRequested) break;

                    //Console.WriteLine($"Вечный цикл | WorkingWithConnection | Клиент {connectedId} получил данные: {recievedData}"); // выдает что recievedData == -1, не знаю почему
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сработал WorkingWithConnection | Ошибка подключения, клиент {connectedId} {ex.Message} ");

                Console.WriteLine("Пробую реконнектится (скорее не реконнектится, а создать новое подключение)");

                DisResConnect(connection, connectedId);
            }
        }

        public void DisResConnect(ClientConnection oldConnection, int badConnectedId)
        {
            try
            {
                if (oldConnection.IsConnected() is false) // возможно сюда надо поставить лок
                {
                    oldConnection._connectionToServerSocket.Shutdown(SocketShutdown.Both);

                    oldConnection._connectionToServerSocket.Close();

                    Task newTask = WorkingWithConnection(badConnectedId, _ct.Token);

                    _connectionTasks.TryUpdate(badConnectedId, newTask, newTask);
                }
                else return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в методе Disconnect | {ex.Message}");
            }
        }
    }
}
