using System.Collections.Concurrent;
using System.Net.Sockets;
using SignalSource.DataManager;
using SignalSource.Listener;
using SignalSource.DataGenerate;

namespace SignalSource.ConnectionManager
{
    public class ConnectionManage
    {
        private ConcurrentDictionary<int, Socket> _clients = new ConcurrentDictionary<int, Socket>();
        private int _countConnections = 0;
        
        private readonly CancellationTokenSource _ct = new();

        public async Task StartTransferDataAsync() // из этого метода каким-то образом надо вытягивать каналы и через них делать передачу данных
        {
            try
            {
                var listener = await StartListenersAsync.StartListenAsync();
                
                do
                {
                    var connection = await listener.AcceptAsync();

                    _clients.TryAdd(Interlocked.Increment(
                        ref _countConnections),
                        connection); // вот тут лежит "канал" а не таска, именно открытый канал с приемником

                    Console.WriteLine($"К серверу подключен клиент {_countConnections}"); // сделать класс канал менеджер. в наружном классе регулировать канал менеджер
                } while (!_ct.IsCancellationRequested);

                // отсюда надо каким-то образом доставать каналы
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }  
    }
}

/*var generate = new DataGeneration(); // примитив передачи данных
var sending = new DataSendingAsync();
for (int i = 0; i < _countConnections; i++)
{
    var b = generate.FirstTypeDataGeneration();
    await sending.DataSendAsync(_clients[i], b);
}*/

