namespace Model
{
    public delegate Task TaskEventHandler(object sender, EventArgs args);

    public delegate Task ValueTaskEventHandler<T>(object sender, ValueEventArgs<T> args);

    public class ValueEventArgs<T> : EventArgs
    {
        public T Value { get; }

        public ValueEventArgs(T value) => Value = value;
    }
}
