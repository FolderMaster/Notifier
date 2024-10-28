namespace Model.Discord.Messages
{
    public record DiscordCommandMessage : DiscordMessage
    {
        public bool IsEphemeral { get; }

        public DiscordCommandMessage(object content, bool isEphemeral = false,
            DiscordEmbed? embed = null) : base(content, embed)
        {
            IsEphemeral = isEphemeral;
        }
    }
}
