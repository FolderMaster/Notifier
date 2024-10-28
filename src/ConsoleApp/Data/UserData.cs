namespace ConsoleApp.Data
{
    public class UserData
    {
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
