namespace Model.Technical
{
    public class LogEventArgs : EventArgs
    {
        public object Content { get; }

        public LogEventArgs(object content) => Content = content;
    }

    public delegate Task LogEventHandler(object sender, LogEventArgs args);
}
