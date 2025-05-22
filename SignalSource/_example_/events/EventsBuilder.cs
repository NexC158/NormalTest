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

    public EventSynchronizator()
    {
        _timerForTypeOne = new Timer(FirstEventActivation, TimeToSendTypeOne, 0, Timeout.Infinite);

        _timerForTypeTwo = new Timer(SecondEventActivation, TimeToSendTypeTwo, 0, Timeout.Infinite);
    }

    private void FirstEventActivation(object state)
    {
        if (_activeConnections >= 1)
        {
            _timerForTypeOne.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
            TimeToSendTypeOne.Invoke();
            Console.WriteLine("сработал второй ивент на отправку данных");

        }
            
    }

    private void SecondEventActivation(object state)
    {
        if (_activeConnections >= 1)
        {
            _timerForTypeTwo.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(_random.Next(400, 600)));
            TimeToSendTypeTwo.Invoke();
            Console.WriteLine("сработал второй ивент на отправку данных");
        }
    }

    public void NotificateOfNewConnection()
    {
        
        Interlocked.Increment(ref _activeConnections);
    }
}
