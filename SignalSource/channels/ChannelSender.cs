using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SignalSource.events;

namespace SignalSource.channels;

internal class ChannelSender
{
    private readonly Socket _clientSocket;

    public ChannelSender(Socket clientSocket)
    {
        _clientSocket = clientSocket;
    }

    public async Task SendTypeOne(byte[] dataToSendType1)
    {
        Console.WriteLine($"Отправлен первый тип | Значение {BitConverter.ToDouble(dataToSendType1, 5)}");
        
        await _clientSocket.SendAsync(dataToSendType1);
    }

    public async Task SendTypeTwo(byte[] dataToSendType2)
    {
        Console.WriteLine($"Отправлен второй тип | Размер {BitConverter.ToUInt32(dataToSendType2, 0)}");

        await _clientSocket.SendAsync(dataToSendType2);
    }

    internal bool IsConnected()
    {
        if (_clientSocket.Connected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

