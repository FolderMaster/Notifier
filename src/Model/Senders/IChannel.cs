namespace Model.Senders
{
    public interface IChannel
    {
        public object Id { get; }

        public bool IsPerson { get; }
    }
}
