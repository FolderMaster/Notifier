using Discord;

namespace Model.Discord.Messages
{
    public class DiscordEmbedField
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public bool IsInline { get; set; }

        public DiscordEmbedField(string name, object value, bool isInline = false)
        {
            Name = name;
            Value = value;
            IsInline = isInline;
        }
    }
}
