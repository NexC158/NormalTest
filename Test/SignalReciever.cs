using System.Collections.Concurrent;
using System.Net.Sockets;

static void Main(string[] args)
{

}

namespace SignalReceiver
{
    public class Receiver
    {
        private readonly int _countOfSources; // количество подключений
        private readonly ConcurrentDictionary<int, Task> _receiverTasks; // или List<Task>

        public Receiver(int countOfSources) // конструктор
        {
            _countOfSources = countOfSources;
        }

        public async Task RunningReceiver(int countOfSources) // создаю подключения
        {
            for (int i = 0; i < _countOfSources; i++)
            {
                _receiverTasks.TryAdd(i, Task.Run(() => ProcessingSourseAsync(i))); // записываю в словарь
            }

            await Task.WhenAll(_receiverTasks.Values); // ожидаю выполнение (не знаю насколько корректна такая запись)

        }

        public async Task ProcessingSourseAsync(int i) // хочу тут обрабатывать данные 
        {
            try
            {
                using var proccessingSource = new TcpClient();

                await proccessingSource.ConnectAsync("localhost", 10000); // ожидаю подключение
                using var stream = proccessingSource.GetStream();
                //using var reader = new StreamReader(stream); это вроде не надо, надо будет потыкать посмотреть
                while (true)
                {
                    // тут буду заниматься обработкой
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


