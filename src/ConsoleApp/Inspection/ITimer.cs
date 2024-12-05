namespace ConsoleApp.Inspection
{
    public interface ITimer
    {
        public Action? Action { get; set; }

        public double Interval { get; set; }

        public void Start();

        public void Stop();
    }
}
