using Model.Senders;

namespace Model.Discord
{
    public class DiscordChannel : IChannel
    {
        public object Id { get; }

        public bool IsPerson { get; }

        public DiscordChannel(ulong id, bool isPerson)
        {
            Id = id;
            IsPerson = isPerson;
        }
    }
}
