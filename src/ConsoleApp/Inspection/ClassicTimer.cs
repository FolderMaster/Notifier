using System.Timers;

using Timer = System.Timers.Timer;

namespace ConsoleApp.Inspection
{
    public class ClassicTimer : ITimer
    {
        private Timer _timer;

        public double Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public Action? Action { get; set; }

        public ClassicTimer()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Action?.Invoke();
        }
    }
}
