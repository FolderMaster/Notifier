using Model.Senders.Bots;

namespace Model.Discord.Commands
{
    public class DiscordCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

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
