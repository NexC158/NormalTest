using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
                    /*var recievedData =*/ await data.DataFilterAsync(connection, connectedId);

                    if (ct.IsCancellationRequested) break;
                    //Console.WriteLine($"Вечный цикл | WorkingWithConnection | Клиент {connectedId} получил данные: {recievedData}"); // выдает что recievedData == -1, не знаю почему
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сработал WorkingWithConnection | Ошибка подключения, клиент {connectedId} {ex.Message} ");

                Console.WriteLine("Пробую реконнектится (скорее не реконнектится, а создать новое подключение)");

                Interlocked.Decrement(ref _currentConnection); // надо придумать как сделать новое подключение без головной боли и плясок с бубном
                                                               // во первых надо удалить из словаря отвалившийся сокет                          
                await Reconnect(connection);                   // во вторых либо на его место засунуть новый, либо просто считать каунтером дальше и не париться

                await Task.Delay(4000);                
            }
        }

        public async Task Reconnect(ClientConnection connection)
        {
            connection = await ClientConnection.ConnectionToServerAsync("127.0.0.1", 10000);
        }
    }
}