using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


internal class EventSynchronizator // ?¯\_(ツ)_/¯
{
    public event Action TimeToSendTypeOne;
    public event Action TimeToSendTypeTwo;

    private Timer _timerOne;
    private Timer _timerTwo;

    Random _random = new Random();

    public void StartTimers()
    {
        _timerOne = new Timer(_ => TimeToSendTypeOne.Invoke(), null, 0, 1000);

        _timerTwo = new Timer(_ => TimeToSendTypeTwo.Invoke(), null, 0, _random.Next(400, 600)); // он не должен запускаться сразу как создастся клиент, этот таймер "глобальный"
    }

    public void StopTimers()
    {
        _timerOne.Dispose();
        _timerTwo.Dispose();
    }
}



// короче тут слежу чтобы отправлялось раз в секунду и два раза в секунду