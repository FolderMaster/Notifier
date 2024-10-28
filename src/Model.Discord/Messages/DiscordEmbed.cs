using Discord;

namespace Model.Discord.Messages
{
    public class DiscordEmbed
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<DiscordEmbedField>? Fields { get; set; }

        public DiscordEmbed(string title, string description,
            IEnumerable<DiscordEmbedField>? fields = null)
        {
            Title = title;
            Description = description;
            Fields = fields;
        }

        internal Embed CreateEmbed()
        {
            var builder = new EmbedBuilder();
            builder.Title = Title;
            builder.Description = Description;
            if (Fields != null)
            {
                foreach (var field in Fields)
                {
                    builder.AddField(field.Name, field.Value, field.IsInline);
                }
            }
            return builder.Build();
        }
    }
}
