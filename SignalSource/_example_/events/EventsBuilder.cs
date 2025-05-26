using System.Threading;

namespace SignalSource._example_.events;


internal class EventsBuilder
{

    private Random _random = new Random();

    public double BuildTypeOne()
    {
        return _random.NextDouble();
    }

    public byte[] BuildTypeTwo()
    {
        var randomLenght = _random.Next(1, 100000);
        var secondTypeData = new byte[randomLenght];
        _random.NextBytes(secondTypeData);
        return secondTypeData;
    }
}


internal class EventSynchronizator // ?¯\_(ツ)_/¯
{
    public event Action TimeToSendTypeOne;
    public event Action TimeToSendTypeTwo;

    private Timer _timerOne;
    private Timer _timerTwo;

    public async Task StartTimers()
    {
        _timerOne = new Timer(_=> TimeToSendTypeOne.Invoke(), null, 0,1000);

        _timerTwo = new Timer(_=> TimeToSendTypeTwo.Invoke(), null, 0, 500);
    }

    public void StopTimers()
    {
        _timerOne.Dispose();
        _timerTwo.Dispose();
    }
}



// короче тут слежу чтобы отправлялось раз в секунду и два раза в секунду