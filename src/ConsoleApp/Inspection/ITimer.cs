using System.Linq.Expressions;

namespace ConsoleApp.Inspection
{
    public interface ITimer
    {
        public TimeSpan Interval { get; set; }

        public void Start();

        public void Stop();
    }
}
