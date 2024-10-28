using Model.Senders.Bots;

namespace Model.Discord.Commands
{
    public class DiscordCommand : ICommand
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<DiscordCommandOption> Options { get; }

        public DiscordCommand(string name, string description,
            IEnumerable<DiscordCommandOption>? options = null)
        {
            Name = name;
            Description = description;
            Options = options ?? Enumerable.Empty<DiscordCommandOption>();
        }
    }
}
