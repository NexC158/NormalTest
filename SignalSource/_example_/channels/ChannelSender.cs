using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.channels
{
    internal class ChannelSender
    {
        private Socket _clientSocket;

        public ChannelSender(Socket clientSocket)
        {
            _clientSocket = clientSocket;
        }

        enum DataTypes : byte // byte??int
        {
            first,
            second
        }

        public async Task SendTypeOne(double value)
        {
            var valueToBytes = BitConverter.GetBytes(value);

            var dataToSend = FormingData(DataTypes.first, valueToBytes);

            await _clientSocket.SendAsync(dataToSend);
        }

        public async Task SendTypeTwo(byte[] data)
        {
            var dataToSend = FormingData(DataTypes.second, data);

            await _clientSocket.SendAsync(dataToSend);
        }

        private byte[] FormingData(DataTypes dataType, byte[] dataToSend)
        {

            var typeBytes = new byte[] { (byte)dataType };

            var sizeBytes = BitConverter.GetBytes(sizeof(uint) + dataToSend.Length);

            var formingBytesArray = new byte[sizeBytes.Length + typeBytes.Length + dataToSend.Length];

            Buffer.BlockCopy(sizeBytes, 0, formingBytesArray, 0, sizeBytes.Length); // мб использовать MemoryStream

            Buffer.BlockCopy(typeBytes, 0, formingBytesArray, sizeBytes.Length, typeBytes.Length);

            Buffer.BlockCopy(dataToSend, 0, formingBytesArray, sizeBytes.Length + typeBytes.Length, dataToSend.Length);

            return formingBytesArray;
        }

    }
}
