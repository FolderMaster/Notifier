namespace Model.Discord
{
    public class DiscordMessage : IMessage
    {
        public object Content { get; private set; }

        public DiscordMessage(object content)
        {
            Content = content;
        }
    }
}
