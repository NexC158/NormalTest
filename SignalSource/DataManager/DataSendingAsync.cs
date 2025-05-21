using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.DataManager
{
    internal class DataSendingAsync
    {
        public async Task DataSendAsync(Socket client, double rndDoubleValue)
        {
            byte[] data = BitConverter.GetBytes(rndDoubleValue); // сюда добавить заполнение ?кадра/хэдера? и отсылать уже его

            var sent = await client.SendAsync(data);
            if (sent == 0) throw new Exception("Error data transfer");

        }
    }
}
