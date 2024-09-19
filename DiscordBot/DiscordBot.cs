using Discord;
using Discord.WebSocket;

namespace Model.Discord
{
    public class DiscordBot : IBot, IService, ILogger
    {
        private readonly DiscordSocketClient _client;

        private readonly string _token;

        private bool _isReady = false;

        public bool IsReady => _isReady;

        public event MessageEventHandler MessageReceived;

        public event LogEventHandler Log;

        public event TaskEventHandler Ready;

        public DiscordBot(string token)
        {
            _token = token;
            _client = new DiscordSocketClient();
            _client.Log += Client_Log;
            _client.Ready += Client_Ready;
            _client.MessageReceived += Client_MessageReceived;
        }

        public async Task Start()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task SendMessage(IMessage message, IUser user)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            if (user.Id is not ulong userId)
            {
                throw new NotImplementedException();
            }
            var discordUser = _client.GetUserAsync(userId).AsTask().Result;
            var socketChannel = (ISocketMessageChannel)discordUser.CreateDMChannelAsync();

            await SendMessage(message, user, socketChannel);
        }

        public async Task SendMessage(IMessage message, IUser user, IChannel channel)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(channel, nameof(channel));

            if (channel.Id is not ulong channelId)
            {
                throw new NotImplementedException();
            }
            var socketChannel = (ISocketMessageChannel)_client.GetChannel(channelId);

            await SendMessage(message, user, socketChannel);
        }

        private async Task SendMessage(IMessage message,
            IUser user, ISocketMessageChannel socketChannel)
        {
            await socketChannel.SendMessageAsync($"<@{user.Id}>, {message.Content}");
        }

        private async Task Client_Ready()
        {
            _isReady = true;
            Ready?.Invoke(this, EventArgs.Empty);
        }

        private async Task Client_Log(LogMessage arg) =>
            Log?.Invoke(this, new LogEventArgs(arg));

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            var message = new DiscordMessage(arg.Content);
            var user = new DiscordUser(arg.Author.Id, arg.Author.IsBot);
            var channel = new DiscordChannel(arg.Channel.Id, arg.Channel is IDMChannel);
            await MessageReceived?.Invoke(this, new MessageEventArgs(message, user, channel));
        }
    }
}
