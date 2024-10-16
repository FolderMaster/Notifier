namespace Model.Discord
{
    public class DiscordUser : IUser
    {
        public object Id { get; private set; }

        public bool IsBot { get; private set; }

        public DiscordUser(ulong id, bool isBot)
        {
            Id = id;
            IsBot = isBot;
        }
    }
}
