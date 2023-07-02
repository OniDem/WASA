using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WASA.Сomplementary
{
    internal class ClockTimer
    {
        private readonly Action<DateTime> _action;
        private CancellationTokenSource? _cts;

        public ClockTimer(Action<DateTime> action)
            => _action = action;

        public async void Start()
        {
            if (_cts != null)
                return;
            try
            {
                using (_cts = new())
                {
                    while (true)
                    {
                        DateTime date = DateTime.Now;
                        _action(date);
                        await Task.Delay(1000 - date.Millisecond, _cts.Token);
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Debug.Fail(ex.ToString());
            }
            _cts = null;
        }

        public void Stop()
         
            => _cts?.Cancel();       
    }
}
