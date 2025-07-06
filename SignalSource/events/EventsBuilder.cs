using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using Timer = System.Timers.Timer;


namespace SignalSource.events;


internal class EventsBuilder
{
    private readonly Random _random = new Random();

    enum DataTypes : byte // byte??int
    {
        first, // добавить можно нотаксептед
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
        var randomLenght = _random.Next(1, 100000);

        var secondTypeData = new byte[randomLenght];

        _random.NextBytes(secondTypeData);

        var dataToSend = FormingData(DataTypes.second, secondTypeData);

        return dataToSend;
    }

    private byte[] FormingData(DataTypes dataType, byte[] dataToSend)
    {
        var typeByte = new byte[] { (byte)dataType };

        var size = sizeof(uint) + typeByte.Length + dataToSend.Length;

        var size2Bytes = BitConverter.GetBytes(size);

        var formingBytesArray = new byte[size];

        Buffer.BlockCopy(size2Bytes, 0, formingBytesArray, 0, size2Bytes.Length); // мб использовать MemoryStream

        Buffer.BlockCopy(typeByte, 0, formingBytesArray, size2Bytes.Length, typeByte.Length);

        Buffer.BlockCopy(dataToSend, 0, formingBytesArray, size2Bytes.Length + typeByte.Length, dataToSend.Length);

        return formingBytesArray;
    }
}


internal class EventSynchronizer : IDisposable 
{
    public event Func<Task>? GlobalTimerType1;

    private CancellationTokenSource? _cts = null;

    public EventSynchronizer()
    {
        _cts = null;
    }

    private async Task ProcessAsyncEvent(Func<Task>? handler1)
    {
        if (handler1 == null)
        {
            return;
        }

        // Delegate[] invocationList = handler1.GetInvocationList();
        //Task[] handlerTasks = new Task[invocationList.Length]; // нафиг массив, сделаю лист

        List<Delegate> invocationList = new List<Delegate>(handler1.GetInvocationList());

        Task[] handlerTask = new Task [invocationList.Count];

        try
        {
            handlerTask = invocationList.Select(async x => // мб переписать на for
            {
                try
                {
                     await ((Func<Task>)x)();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch при обертке элемента {x} handlerTasks в ProcessAsyncEvent | {ex.Message}");
                }
            }).Where(x => !x.IsFaulted ).ToArray(); // https://stackoverflow.com/questions/1105990/is-it-better-to-call-tolist-or-toarray-in-linq-queries что лист что массив работают одинаково
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch при handlerTask = invocationList в ProcessAsyncEvent | {ex.Message}");
            
        }

        await Task.WhenAll(handlerTask);
    }

    public void Dispose()
    {
        StopInternal();
    }

    public void Stop()
    {
        StopInternal();
    }

    private void StopInternal()
    {
        var cts = Interlocked.Exchange(ref _cts, null);
        cts?.Cancel();
    }

    public void StartIfNotYet()
    {
        // fast check
        if(_cts is not null)
        {
            return;
        }

        // long check
        var newCts = new CancellationTokenSource();
        if ( null == Interlocked.CompareExchange(ref _cts, newCts, null))
        {
            _ = Processing(newCts.Token);
        }
    }

    public void StartAndStopPrev()
    {
        var newCts = new CancellationTokenSource();
        var oldCts = Interlocked.Exchange(ref _cts, newCts);
        if (oldCts is not null)
        {
            oldCts.Cancel();
        }

        _ = Processing(newCts.Token);
    }


    private async Task Processing(CancellationToken stop)
    {
        while (stop.IsCancellationRequested is false)
        {
            await Task.Delay(1000);
            await ProcessAsyncEvent(GlobalTimerType1);
        }
    }

}