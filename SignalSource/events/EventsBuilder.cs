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

        return dataToSend;
    }

    private byte[] FormingData(DataTypes dataType, byte[] dataToSend)
    {
        var typeByte = new byte[] { (byte)dataType };

        int size = sizeof(uint) + typeByte.Length + dataToSend.Length;

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

    private readonly Random _random = new Random();

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

        List<Task> handlerTask = new List<Task>(invocationList.Count);

        try
        {
            handlerTask = invocationList.Select(async x =>
            {
                try
                {
                     await ((Func<Task>)x)();
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch при обертке элемента {x} handlerTasks в ProcessAsyncEvent | {ex.Message}");
                }
            }).Where(x => !x.IsFaulted ).ToList(); // моя попытка избежать ошибки, надо додумывать


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch при handlerTask = invocationList в ProcessAsyncEvent | {ex.Message}");
            
        }

        await Task.WhenAll(handlerTask);
    }

        

/*
        try
        {
            for (int i = 0; i < invocationList.Count; i++)
            {
                try
                {
                    handlerTasks[i] = ((Func<Task>)invocationList[i])();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сработал catch при обертке handlerTasks[i] в ProcessAsyncEvent | {ex.Message}");
                }
                finally
                {
                    handlerTasks[i].Dispose();
                }
            }
            await Task.WhenAll(handlerTasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сработал catch в ProcessAsyncEvent | {ex.Message}");
        }
    }*/

    /*public static async Task ProcessAsyncEvent<TEventData>(Func<TEventData, Task>? handler, TEventData args)
    {
        if (handler == null)
        {
            return;
        }

        Delegate[] invocationList = handler.GetInvocationList();

        // FUTURE: optimize 1: use stack (span) for the array; optimize 2: simple case if only one handler
        Task[] handlerTasks = new Task[invocationList.Length];

        for (int i = 0; i < invocationList.Length; i++)
        {
            handlerTasks[i] = ((Func<TEventData, Task>)invocationList[i])(args);
        }
        await Task.WhenAll(handlerTasks);
    }*/

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