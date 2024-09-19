namespace Model.Discord
{
    public class DiscordBotCommandHelper : BotCommandHelper
    {
        private readonly IDictionary<ICommand, DiscordCommandDelegate> _commandsTasks;

        public DiscordBotCommandHelper(DiscordBot bot,
            IDictionary<ICommand, DiscordCommandDelegate> commandsTasks) :
            base(bot, commandsTasks.Keys) => _commandsTasks = commandsTasks;

        protected override async Task OnCommandReceived(object sender, CommandEventArgs args)
        {
            var discordArgs = (DiscordCommandEventArgs)args;
            await _commandsTasks[args.Command](discordArgs);
        }
    }

    public delegate Task DiscordCommandDelegate(DiscordCommandEventArgs args);
}
