namespace Model.Discord
{
    public class DiscordBotMessage : DiscordMessage
    {
        public bool IsEphemeral { get; }

        public DiscordBotMessage(object content, bool isEphemeral) : base(content) =>
            IsEphemeral = isEphemeral;
    }
}
