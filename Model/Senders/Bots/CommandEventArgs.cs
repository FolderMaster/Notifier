namespace Model.Senders.Bots
{
    public class CommandEventArgs
    {
        public ICommand Command { get; private set; }

        public IUser User { get; private set; }

        public IChannel Channel { get; private set; }

        public CommandEventArgs(ICommand command, IUser user, IChannel channel)
        {
            Command = command;
            User = user;
            Channel = channel;
        }
    }

    public delegate Task CommandEventHandler(object sender, CommandEventArgs args);
}
