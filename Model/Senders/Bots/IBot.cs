using Model.Senders;

namespace Model.Senders.Bots
{
    public interface IBot : ISender
    {
        public event MessageEventHandler MessageReceived;

        public event CommandEventHandler CommandReceived;

        public Task SendMessage(IMessage message, IUser user, IChannel channel);

        public Task RegisterCommands(IEnumerable<ICommand> commands);
    }
}
