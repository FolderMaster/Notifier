using Model.Technical;

namespace Model.Senders.Bots
{
    public abstract class BotCommandHelper
    {
        private readonly IBot _bot;

        public BotCommandHelper(IBot bot, IEnumerable<ICommand> commands)
        {
            _bot = bot;
            _bot.CommandReceived += Bot_CommandReceived;
            if (_bot is IService service)
            {
                if (service.IsReady)
                {
                    _bot.RegisterCommands(commands);
                }
                else
                {
                    service.Ready += (sender, args) => _bot.RegisterCommands(commands);
                }
            }
        }

        protected async Task Bot_CommandReceived(object sender, CommandEventArgs args)
        {
            await OnCommandReceived(sender, args);
        }

        protected abstract Task OnCommandReceived(object sender, CommandEventArgs args);
    }
}
