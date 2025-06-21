using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SignalRecieverAnalyzer.Data
{
    internal class DataAnalyze
    {
        private readonly Channel<double> _channel;

        private double? _previousValue;

        //private int _previousClientId;

        private int _shiftId = 0;

        public DataAnalyze(int countConnections = 100)
        {
            var options = new BoundedChannelOptions(countConnections)
            {
                SingleReader = true,
                SingleWriter = false,
                FullMode = BoundedChannelFullMode.Wait
            };

            _channel = Channel.CreateBounded<double>(options);

            //StartAnalyse();

            Task.Run(ProcessDataAsync);
        }

        public ValueTask WriteToChannelAsync(double value, CancellationToken ct)
        {
            Console.WriteLine($"Запись в канал {value}");
            return _channel.Writer.WriteAsync(value, ct);
        }

        public async Task ProcessDataAsync()
        {
            try
            {
                while (await _channel.Reader.WaitToReadAsync())
                {
                    while (_channel.Reader.TryRead(out var value))
                    {
                        if (_previousValue.HasValue)
                        {
                            var troyka = new double[3];
                            double currentValue = value;
                            Console.WriteLine($"Чтение из канала {value}");
                            troyka[0] = _previousValue.Value;

                            double difference = Math.Abs(_previousValue.Value - currentValue);

                            if (difference > 0.5)
                            {
                                troyka[1] = Interlocked.Increment(ref _shiftId);
                                troyka[2] = difference;
                            }
                            Console.WriteLine($"Результат тройки:  {troyka[0]} | {troyka[1]} | {troyka[2]}  _previousValue{_previousValue} | currentValue{currentValue}");
                        }
                        _previousValue = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в методе ProcessDataAsync | {ex.Message}");
            }
        }
    }
}
