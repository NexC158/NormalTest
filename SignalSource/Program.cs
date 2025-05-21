using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SignalSource.Listener;
using SignalSource.ConnectionManager;
using SignalSource.DataManager;
using SignalSource.DataGenerate;

namespace SignalSource
{
    public class Program // server 
    { 
        public static async Task Main(string[] args)
        {
            var connect = new ConnectionManage();
            var data = new DataGeneration();
            var sync = new StartListenersAsync();
            
            await connect.StartTransferDataAsync();


           /*1) обертка сокета, спросить еще раз, не понял
             2) из него надо делать оберку или че как почему он должен быть приватным ClientsConnectionSocket?
             3) сделать channelSender - прокси - это будет обертка для сокета, спросить что имеется ввиду, и куда его надо засунуть
             4) как доставать каналы из ConnectionManager?
             5) 

                сделал генерацию данных, спросить пральна или нет*/
        }
    }
}

// 1) это класс снаружи моих сокетов внутри держит сокет. он прячет все. Channel Sender




// в тайпе 1 байт 


// big ending or little ending  // делать по машинному