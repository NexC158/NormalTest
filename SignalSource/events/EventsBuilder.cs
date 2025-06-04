using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
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


internal class EventSynchronizer : IDisposable // ?¯\_(ツ)_/¯ это класс издатель, он определяет когда событие должно выполняться 
{
    public event EventHandler? GlobalTimerType1; // для каждого активного канала раз в секунду

    private readonly Random _random = new Random();

    private CancellationTokenSource? _cts = null;

    public EventSynchronizer()
    {
        _cts = null;
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
            GlobalTimerType1?.Invoke(this, EventArgs.Empty);
        }
    }

}