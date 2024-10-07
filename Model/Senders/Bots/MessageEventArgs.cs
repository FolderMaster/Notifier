using Model.Senders;

namespace Model.Senders.Bots
{
    public class MessageEventArgs
    {
        public IMessage Message { get; }

        public IUser User { get; }

        public IChannel Channel { get; }

        public MessageEventArgs(IMessage message, IUser user, IChannel channel)
        {
            Message = message;
            User = user;
            Channel = channel;
        }
    }

    public delegate Task MessageEventHandler(object sender, MessageEventArgs args);
}
