using Discord;
using System.Collections.ObjectModel;

using IChannel = Model.Senders.IChannel;

namespace Model.Discord.Commands
{
    public class DiscordCommandOption
    {
        public static ReadOnlyDictionary<Type, ApplicationCommandOptionType> TypeDictionary =
            new(new Dictionary<Type, ApplicationCommandOptionType>()
            {
                [typeof(long)] = ApplicationCommandOptionType.Integer,
                [typeof(double)] = ApplicationCommandOptionType.Number,
                [typeof(bool)] = ApplicationCommandOptionType.Boolean,
                [typeof(string)] = ApplicationCommandOptionType.String,
                [typeof(IUser)] = ApplicationCommandOptionType.User,
                [typeof(IChannel)] = ApplicationCommandOptionType.Channel
            });

        public string Name { get; }

        public string Description { get; }

        public bool IsRequired { get; }

        public Type Type { get; }

        public DiscordCommandOption(string name, string description,
            Type type, bool isRequired = false)
        {
            if (!TypeDictionary.ContainsKey(type))
            {
                throw new ArgumentException(nameof(type));
            }
            Name = name;
            Description = description;
            Type = type;
            IsRequired = isRequired;
        }
    }
}
