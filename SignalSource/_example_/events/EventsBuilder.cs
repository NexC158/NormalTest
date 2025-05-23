using System.Linq.Expressions;
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
        var randomLenght = _random.Next(1, 1000);
        var secondTypeData = new byte[randomLenght];
        _random.NextBytes(secondTypeData);
        return secondTypeData;
    }
}


internal class EventSynchronizator
{
    public event Action TimeToSendTypeOne;

    public event Action TimeToSendTypeTwo;

    private readonly Timer _timerForTypeOne;

    private readonly Timer _timerForTypeTwo;

    private Random _random = new Random();

    private int _activeConnections = 0;



    public EventSynchronizator() // это у меня издатель
    {
        _timerForTypeOne = new Timer(FirstEventActivation, null, Timeout.Infinite, Timeout.Infinite);

        _timerForTypeTwo = new Timer(SecondEventActivation, null, Timeout.Infinite, Timeout.Infinite);
    }

    private void FirstEventActivation(object state)
    {
        if (_activeConnections >= 1)
        {
            TimeToSendTypeOne.Invoke();
            Console.WriteLine("сработал первый ивент на отправку данных");

            _timerForTypeOne.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(10000));
        }
        else
        {
            Console.WriteLine($"Проблемка#1");
        }
    }

    private void SecondEventActivation(object state)
    {
        if (_activeConnections >= 1)
        {
            TimeToSendTypeTwo.Invoke();
            Console.WriteLine("сработал второй ивент на отправку данных");
            _timerForTypeTwo.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(_random.Next(4000, 6000)));
        }
        else
        {
            Console.WriteLine($"Проблемка#2");
        }
    }

    public void NotificateOfNewConnection()
    {
        Interlocked.Increment(ref _activeConnections);
        _timerForTypeOne.Change(0, 0);
        _timerForTypeTwo.Change(0, 0);

        /*_timerForTypeOne.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        _timerForTypeTwo.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(_random.Next(400, 600)));*/
    }

    public void UnsubscribeConnection()
    {
        Interlocked.Decrement(ref _activeConnections);
    }
}