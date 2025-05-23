using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalRecieverAnalyzer.Connection;
using SignalRecieverAnalyzer.DataRecieveAndAnalyzer;


namespace SignalRecieverAnalyzer.Working
{
    internal class StartWorking
    {
        private ConcurrentDictionary<int, Task> _connectionTasks = new ConcurrentDictionary<int, Task>();
        private int maxConnections = 100; // надо куда-нибудь вынести, чтобы сразу было понятно сколько подключений
        private int currentConnection = 0;
        private readonly CancellationTokenSource _ct = new();

        public async Task StartRecieveAsync()
        {
            for (int i = 0; i < maxConnections; i++)
            {
                var connectedId = Interlocked.Increment(ref currentConnection);
                Console.WriteLine($"Начинаю попытку подключения для {connectedId} клиента");
                _connectionTasks.TryAdd(connectedId, WorkingWithConnection(connectedId, _ct.Token));
            }
            await Task.WhenAll(_connectionTasks.Values);
        }

        private async Task WorkingWithConnection(int connectedId, CancellationToken ct)
        {
            var connect = new ConnectionToServer();
            var data = new DataProccesing();
            try
            {
                using var connection = await connect.ConnectionAsync();

                Console.WriteLine($"Подключен клиент: {connectedId}");

                try
                {
                    while (true)
                    {
                        var recievedData = await data.RecieveDoubleAsync(connection);
                        Console.WriteLine($"Клиент {connectedId} получил данные: {recievedData}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка получения данных клиентом: {connectedId}. {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения, клиент {connectedId} {ex.Message} ");

                // сюда пихнуть что-то для реконнекта
            }

            if (!ct.IsCancellationRequested)
            {
                await Task.Delay(1000, ct); // или сюда пихнуть что-то для реконнекта
                Console.WriteLine("Сработал CancellationTocken");
            }


        }
    }
}
