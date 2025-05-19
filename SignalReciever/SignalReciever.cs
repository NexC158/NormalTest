using System.Collections.Concurrent;
using System.Net.Sockets;

namespace SignalRecieverAnalyzer
{
    public class SignalReciever // ???????????????????????????????
    {

        //public static async Task<double> Analizator(Socket socket)
        //{
        //    await socket.ConnectAsync("127.0.0.1", 10000);
        //    return await RecieveData(socket);

        //}
        //public static async Task<double> RecieveData(Socket socket)
        //{
        //    var buffer = new byte[32];
        //    await socket.ReceiveAsync(buffer);

        //    var recievedBytes = Converter(buffer);
        //    return recievedBytes;
        //}

        //public static double Converter(byte[] bytesToConvert)
        //{
        //    return BitConverter.ToDouble(bytesToConvert, 0);
        //}

        //public static async Task PeriodPeredachi(Task task)
        //{
        //    await task;
        //    await Task.Delay(1000);
        //}

        //public static async Task Peredacha(Socket socket)
        //{
        //    using var connection = await CreateNewConnectionAsync(socket);
        //    await SendDataAsync(connection);
        //}


        //public static async Task<Socket> CreateNewConnectionAsync(Socket socket)
        //{
        //    using Socket connection = await socket.AcceptAsync();
        //    return connection;
        //}

        //public static async Task SendDataAsync(Socket socket)
        //{
        //    var dataToSend = GenerateMessage();
        //    await socket.SendAsync(dataToSend);
        //}

        //public static byte[] GenerateMessage()
        //{
        //    var randomDouble = RandomGeneration();

        //    return BitConverter.GetBytes(randomDouble);
        //}

        //public static double RandomGeneration()
        //{
        //    var random = new Random();
        //    var result = random.NextDouble();
        //    return result;
        //}
    }
}
