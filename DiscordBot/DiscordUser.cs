namespace Model.Discord
{
    public class DiscordUser : IUser
    {
        public object Id { get; }

        public bool IsBot { get; }

        public DiscordUser(ulong id, bool isBot)
        {
            Id = id;
            IsBot = isBot;
        }
    }
}
