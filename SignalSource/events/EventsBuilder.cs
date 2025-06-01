using System;
using System.Net.Sockets;
using System.Timers;
using Timer = System.Timers.Timer;


namespace SignalSource.events;


internal class EventsBuilder
{

    private Random _random = new Random();

    enum DataTypes : byte // byte??int
    {
        first,
        second
    }

    public byte[] BuildTypeOne()
    {
        var valueToBytes = BitConverter.GetBytes(_random.NextDouble());

        var dataToSend = FormingData(DataTypes.first, valueToBytes);

        return dataToSend;
    }

    public byte[] BuildTypeTwo()
    {
        var randomLenght = _random.Next(1, 1000);

        var secondTypeData = new byte[randomLenght];

        _random.NextBytes(secondTypeData);

        var dataToSend = FormingData(DataTypes.second, secondTypeData);

        return secondTypeData;
    }
    private byte[] FormingData(DataTypes dataType, byte[] dataToSend)
    {
        var typeByte = new byte[] { (byte)dataType };

        int size2 = sizeof(uint) + typeByte.Length + dataToSend.Length;

        var size2Bytes = BitConverter.GetBytes(size2);

        var formingBytesArray = new byte[size2];

        Buffer.BlockCopy(size2Bytes, 0, formingBytesArray, 0, size2Bytes.Length); // мб использовать MemoryStream

        Buffer.BlockCopy(typeByte, 0, formingBytesArray, size2Bytes.Length, typeByte.Length);

        Buffer.BlockCopy(dataToSend, 0, formingBytesArray, size2Bytes.Length + typeByte.Length, dataToSend.Length);

        return formingBytesArray;
    }
}


internal class EventSynchronizator // ?¯\_(ツ)_/¯ это класс издатель, он определяет когда событие должно выполняться 
{
    public event EventHandler GlobalTimerType1; // для каждого активного канала раз в секунду

    public event EventHandler PersonalTimerType2; // в каждом канале независимо

    private static Random _random = new Random();

    public void OnGlobalTimerType1(object sender, EventArgs e) // вот тут sender это отправитель события, и я думаю что это таймер
    {
        GlobalTimerType1?.Invoke(sender, e);
    }

    public void OnPersonalTimerType2(object sender, EventArgs e)
    {
        PersonalTimerType2?.Invoke(sender, e);
    }
}