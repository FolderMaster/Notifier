namespace Model.Discord
{
    public class DiscordCommandEventArgs : CommandEventArgs
    {
        private readonly SendMessageDelegate _sendMessage;

        public IDictionary<string, object> Options { get; }

        public DiscordCommandEventArgs(ICommand command, IUser user,
            IChannel channel, IDictionary<string, object> options,
            SendMessageDelegate sendMessage) : base(command, user, channel)
        {
            Options = options;
            _sendMessage = sendMessage;
        }

        public async Task SendMessage(IMessage message) =>
            await _sendMessage.Invoke(message);
    }

    public delegate Task SendMessageDelegate(IMessage message);
}
