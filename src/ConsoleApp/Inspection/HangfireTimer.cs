using Hangfire;
using System.Linq.Expressions;

namespace ConsoleApp.Inspection
{
    public class HangfireTimer<T> : ITimer
    {
        private static int _nextId = 0;

        private string? _jobId;

        public Expression<Func<T, Task>>? TaskExpression { get; set; }

        public TimeSpan Interval { get; set; }

        public void Start()
        {
            if (TaskExpression != null)
            {
                _jobId = _nextId.ToString();
                RecurringJob.AddOrUpdate(_jobId, TaskExpression,
                    (Interval.Minutes > 0 ? $"*/{Interval.Minutes}" : " *") +
                    (Interval.Hours > 0 ? $"*/{Interval.Hours}" : " *") +
                    " * * *");
                ++_nextId;
            }
        }

        public void Stop()
        {
            if (_jobId != null)
            {
                RecurringJob.RemoveIfExists(_jobId);
            }
        }
    }
}
