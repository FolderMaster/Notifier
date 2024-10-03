using Discord;
using Discord.Net.Rest;
using Discord.Net.WebSockets;
using Discord.WebSocket;
using System.Net;

namespace Model.Discord
{
    public class DiscordBot : IBot, IService, ILogger
    {
        private readonly DiscordSocketClient _client;

        private Dictionary<ulong, ICommand> _commands;

        private readonly string _token;

        private bool _isReady = false;

        public bool IsReady => _isReady;

        public event MessageEventHandler MessageReceived;

        public event CommandEventHandler CommandReceived;

        public event LogEventHandler Log;

        public event TaskEventHandler Ready;

        public DiscordBot(string token, IWebProxy? proxy = null)
        {
            _token = token;
            var settings = new DiscordSocketConfig();
            if (proxy != null)
            {
                settings.WebSocketProvider = DefaultWebSocketProvider.Create(proxy);
                settings.RestClientProvider = DefaultRestClientProvider.Create(true);
            }
            _client = new DiscordSocketClient(settings);
            _client.Log += Client_Log;
            _client.Ready += Client_Ready;
            _client.MessageReceived += Client_MessageReceived;
            _client.SlashCommandExecuted += Client_SlashCommandExecuted;
        }

        public async Task Start()
        {
            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        public Task<bool> CheckUserId(object userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));

            return _client.GetUserAsync((ulong)userId).AsTask().
                ContinueWith(task => task.Result != null);
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
            var dmChannel = discordUser.CreateDMChannelAsync().Result;

            await SendMessage(message, user, dmChannel);
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
            var messageChannel = (IMessageChannel)_client.GetChannel(channelId);

            await SendMessage(message, user, messageChannel);
        }

        public Task RegisterCommands(IEnumerable<ICommand> commands)
        {
            ArgumentNullException.ThrowIfNull(commands, nameof(commands));

            var commandsProperties = new List<ApplicationCommandProperties>();
            foreach (var command in commands)
            {
                var commandProperties = CreateCommandProperties(command);
                commandsProperties.Add(commandProperties);
            }

            var applicationCommands = _client.BulkOverwriteGlobalApplicationCommandsAsync
                (commandsProperties.ToArray()).Result;
            _commands = applicationCommands.Select(a => a.Id).Zip(commands).
                ToDictionary(c => c.First, c => c.Second);

            return Task.CompletedTask;
        }

        private ApplicationCommandProperties CreateCommandProperties(ICommand command)
        {
            var commandBuilder = new SlashCommandBuilder().WithName(command.Name);
            if (command is DiscordCommand discordCommand)
            {
                commandBuilder.WithDescription(discordCommand.Description);
                var optionsBuilders = new List<SlashCommandOptionBuilder>();
                foreach (var option in discordCommand.Options)
                {
                    var optionBuilder = new SlashCommandOptionBuilder().
                        WithName(option.Name).WithDescription(option.Description).
                        WithRequired(option.IsRequired).
                        WithType(DiscordCommandOption.TypeDictionary[option.Type]);
                    optionsBuilders.Add(optionBuilder);
                }
                commandBuilder.AddOptions(optionsBuilders.ToArray());
            }
            return commandBuilder.Build();
        }

        private async Task SendMessage(IMessage message,
            IUser user, IMessageChannel messageChannel)
        {
            if (messageChannel is IDMChannel dmChannel)
            {
                await dmChannel.SendMessageAsync(message.Content?.ToString());
            }
            else
            {
                await messageChannel.SendMessageAsync($"<@{user.Id}>, {message.Content}");
            }
        }

        private async Task Client_Ready()
        {
            _isReady = true;

            if (Ready != null)
            {
                await Ready.Invoke(this, EventArgs.Empty);
            }
        }

        private async Task Client_Log(LogMessage arg)
        {
            if (Log != null)
            {
                await Log.Invoke(this, new LogEventArgs(arg));
            }
        }
        
        private async Task Client_MessageReceived(SocketMessage arg)
        {
            if (MessageReceived != null)
            {
                var message = new DiscordMessage(arg.Content);
                var user = new DiscordUser(arg.Author.Id, arg.Author.IsBot);
                var channel = new DiscordChannel(arg.Channel.Id, arg.Channel is IDMChannel);

                await MessageReceived.Invoke(this, new MessageEventArgs(message, user, channel));
            }
        }

        private async Task Client_SlashCommandExecuted(SocketSlashCommand arg)
        {
            if (CommandReceived != null)
            {
                var command = _commands[arg.CommandId];
                var user = new DiscordUser(arg.User.Id, arg.User.IsBot);
                var channel = new DiscordChannel(arg.Channel.Id, arg.Channel is IDMChannel);
                var options = arg.Data.Options.ToDictionary(o => o.Name, o => o.Value);

                var sendMessage = (SendMessageDelegate)(async (IMessage message) =>
                {
                    var isEphemeral = false;
                    if (message is DiscordBotMessage discordBotMessage)
                    {
                        isEphemeral = discordBotMessage.IsEphemeral;
                    }
                    await arg.RespondAsync(message.Content?.ToString(), ephemeral: isEphemeral);
                });

                await CommandReceived.Invoke(this,
                    new DiscordCommandEventArgs(command, user, channel, options, sendMessage));
            }
        }
    }
}
