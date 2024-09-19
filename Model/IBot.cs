namespace Model
{
    public interface IBot : ISender
    {
        public event MessageEventHandler MessageReceived;

        public Task SendMessage(IMessage message, IUser user, IChannel channel);
    }
}
