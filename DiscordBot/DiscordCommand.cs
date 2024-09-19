namespace Model.Discord
{
    public class DiscordCommand : ICommand
    {
        public string Name { get; }

        public string Description { get; }

        public IEnumerable<DiscordCommandOptions> Options { get; }
    }
}
