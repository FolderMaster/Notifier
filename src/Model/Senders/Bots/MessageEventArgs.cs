namespace Model.Senders.Bots
{
    public class MessageEventArgs
    {
        public IMessage Message { get; private set; }

        public IUser User { get; private set; }

        public IChannel Channel { get; private set; }

        public MessageEventArgs(IMessage message, IUser user, IChannel channel)
        {
            Message = message;
            User = user;
            Channel = channel;
        }
    }

    public delegate Task MessageEventHandler(object sender, MessageEventArgs args);
}
