using Model.Senders;

namespace Model.Discord.Messages
{
    public record DiscordMessage : IMessage
    {
        public object Content { get; set; }

        public DiscordEmbed? Embed { get; set; }

        public DiscordMessage(object content, DiscordEmbed? embed = null)
        {
            Content = content;
            Embed = embed;
        }
    }
}
