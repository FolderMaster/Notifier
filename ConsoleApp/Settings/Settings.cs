namespace ConsoleApp.Settings
{
    public class Settings
    {
        public DataBaseSettings DataBase { get; set; }

        public DiscordSettings Discord { get; set; }

        public EmailSettings Email { get; set; }

        public JiraSettings Jira { get; set; }
    }
}
