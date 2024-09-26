using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data
{
    [PrimaryKey(nameof(Id))]
    public class UserData
    {
        public int Id { get; set; }

        public ulong DiscordId { get; set; }

        public string? EmailAddress { get; set; }

        public string JiraId { get; set; }

        public UserData() { }

        public UserData(ulong discordId, string? emailAddress, string jiraId)
        {
            DiscordId = discordId;
            EmailAddress = emailAddress;
            JiraId = jiraId;
        }
    }
}
