namespace Model
{
    public class CommandEventArgs
    {
        public ICommand Command { get; }

        public IUser User { get; }

        public IChannel Channel { get; }

        public CommandEventArgs(ICommand command, IUser user, IChannel channel)
        {
            Command = command;
            User = user;
            Channel = channel;
        }
    }

    public delegate Task CommandEventHandler(object sender, CommandEventArgs args);
}
