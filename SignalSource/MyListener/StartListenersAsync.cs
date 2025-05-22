
using System.Net;
using System.Net.Sockets;


namespace SignalSource.Listener
{
    public class StartListenersAsync
    {
        public static async Task<Socket> StartListenAsync() // Он статический и должен возвращать объект 
        {
            var port = 10000; 
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(100);
            Console.WriteLine($"Начинаю слушать порт: {port} и ip: {IPAddress.Any}");
            
            return listener;
        }

        public async Task<Socket> ClientsConnectionSocket(Socket listener) // вообще это по идее должно быть в другом месте 
        {
            var connection = await listener.AcceptAsync(); // приватный метод который принимает сокет и возвращает уже "свой" сокет, а значит влезть в него не получится
            return connection;
        }
    }
}
// сделать channelSender - прокси - это будет обертка для сокета
