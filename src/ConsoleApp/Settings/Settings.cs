namespace ConsoleApp.Settings
{
    public class Settings
    {
        public DataBaseSettings DataBase { get; set; }

        public IEnumerable<SenderSettings> Senders { get; set; }

        public IEnumerable<RuleSettings> Rules { get; set; }

        public JiraSettings Jira { get; set; }
    }
}
