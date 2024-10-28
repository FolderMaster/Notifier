using Model.Senders;

namespace Model.Discord
{
    public record DiscordChannel : IChannel
    {
        public object Id { get; private set; }

        public bool IsPerson { get; private set; }

        public DiscordChannel(ulong id, bool isPerson)
        {
            Id = id;
            IsPerson = isPerson;
        }
    }
}
