using System.Net;
using System.Net.Sockets;

static void Main(string[] args)
{

}

namespace SignalSource
{
    public class Source
    {
        private readonly int _port = 10000; // порт
        private readonly int _date2PerSecond; // плотность сигналов
        private int _activeSources = 0; // активные клиенты
        private object _lockObject = new object(); // примитив синхронизации
        private TcpListener listener;
        // возможно сюда нужно будет добавить лист или словарь для управления клиентами
        public Source(int date2PerSecond) // конструктор
        {
            _date2PerSecond = date2PerSecond;
        }



        public async Task RunningAsync()
        {
            listener = new TcpListener(IPAddress.Any, _port);
            listener.Start(); // запускаю прослушивание порта

            // вот тут хочу запустить глобальный таймер на первое событие и сделать чтобы его срабатывание вызывал метод рандомайзера NextDouble

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync(); // подключение нового клиента
                _activeSources++; // счетчик в плюс

                lock (_lockObject) // по идее блокирую поток для изменения счетчика, но что-то мне подсказывает что оно работает по другому
                {
                    if (_activeSources > 100) // закрываю клиента
                    {
                        _activeSources--;
                        client.Close();
                    }
                }
            }
        }

        public async Task ProcessingClientAsync(TcpClient client, CancellationToken ct) // не уверен
        {
            try
            {
                using (client)
                {
                    using var stream = client.GetStream();
                    {
                        while (true) // ct.IsCancellationRequested == false 
                        {
                            // здесь хочу сделать передачу первого события
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
