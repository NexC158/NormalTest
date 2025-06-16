using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalRecieverAnalyzer.Data
{
    internal class DataAnalyze : IDisposable
    {
        private readonly Channel<(double value, int clientId)> _channel;

        private double? _previousValue;

        private int _previousClientId;

        private int _shiftId = 0;

        private CancellationTokenSource _cts;

        public DataAnalyze(int maxConnections = 100)
        {
            var options = new BoundedChannelOptions(maxConnections)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.Wait
            };

            _channel = Channel.CreateBounded<(double, int)>(options);

            _cts = new CancellationTokenSource();

            StartAnalyse();
        }


        private void StartAnalyse()
        {
            Task.Run(async () =>
            {
                while(await _channel.Reader.WaitToReadAsync(_cts.Token))
                {
                    while(_channel.Reader.TryRead(out var value))
                    {
                        ProcessDataAsync(value.value, value.clientId);
                    }
                }
            }, _cts.Token);
             

        }
        public ConcurrentQueue<double> _doubleQueue = new ConcurrentQueue<double>();

        public async Task ProcessDataAsync(double value, int clientId)
        {
            try
            {
                
                    if (_previousValue.HasValue is false)
                    {
                        _previousValue = value;

                        _previousClientId = clientId;
                    }
                    else
                    {
                        var troyka = new double[3];

                        troyka[0] = _previousValue.Value; // тут вроде не та логика

                        troyka[1] = value;

                        if (Math.Abs(troyka[0] - troyka[1]) > 0.5)
                        {
                            troyka[2] = Interlocked.Increment(ref _shiftId);
                        }
                        else
                        {
                            troyka[2] = (double)_shiftId;
                        }

                        Console.WriteLine($"Результат тройки: клиент {_previousClientId} значение {_previousValue} | клиент {clientId} значение {value} | сдвиг {_shiftId}");
                    }
                    
                
                await Task.CompletedTask;
            }

            catch (Exception ex) 
            {
                Console.WriteLine($"Сработал catch в методе ProcessDataAsync | {ex.Message}");
            }

        }

        public void MyEnqueue(double value)
        {
            _doubleQueue.Enqueue(value);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
