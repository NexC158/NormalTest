using System.Collections.Concurrent;
using System.Net.Sockets;

namespace SignalRecieverAnalyzer
{
    public class SignalReciever // ???????????????????????????????
    {
        
        public static async Task<double> Analizator(Socket socket)
        {
            await socket.ConnectAsync("127.0.0.1", 10000);
            return await RecieveData(socket);

        }
        public static async Task<double> RecieveData(Socket socket)
        {
            var buffer = new byte[32];
            await socket.ReceiveAsync(buffer);

            var recievedBytes = Converter(buffer);
            return recievedBytes;
        }

        public static double Converter(byte[] bytesToConvert)
        {
            return BitConverter.ToDouble(bytesToConvert, 0);
        }
    }
}
