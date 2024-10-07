using Model.Senders;

namespace Model.Discord.Messages
{
    public class DiscordMessage : IMessage
    {
        public object Content { get; }

        public DiscordEmbed? Embed { get; }

        public DiscordMessage(object content, DiscordEmbed? embed = null)
        {
            Content = content;
            Embed = embed;
        }
    }
}
