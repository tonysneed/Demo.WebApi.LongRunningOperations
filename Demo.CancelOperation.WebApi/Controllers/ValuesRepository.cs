using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.CancelOperation.WebApi.Controllers
{
    public class ValuesRepository
    {
        private int _progress;
        private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private readonly List<string> _result = new List<string>();

        public int Interval { get; }
        public int Count { get; }

        public ValuesRepository(int interval, int count)
        {
            Interval = interval;
            Count = count;
        }

        public void Start()
        {
            // Run this in the background
            Task.Run(async () =>
            {
                for (int i = 0; i < Count; i++)
                {
                    int num = i + 1;
                    await Work(num);
                }
                _progress = -1;
            });
        }

        public int Progress()
        {
            return _progress;
        }

        public void Cancel()
        {
            _progress = 0;
            _cancelTokenSource.Cancel();
        }

        public string[] GetResult()
        {
            if (_progress == -1)
                return _result.ToArray();
            return new string[0];
        }

        private async Task Work(int num)
        {
            await Task.Delay(TimeSpan.FromSeconds(Interval), _cancelTokenSource.Token);
            if (_cancelTokenSource.IsCancellationRequested) return;
            _result.Add($"value{num}");
            _progress++;
        }
    }
}