using System.Linq.Expressions;

namespace ConsoleApp.Inspection
{
    public interface ITimer
    {
        public Expression<Func<Task>>? TaskExpression { get; set; }

        public TimeSpan Interval { get; set; }

        public void Start();

        public void Stop();
    }
}
