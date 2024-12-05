using ConsoleApp.Settings.Executors;
using ConsoleApp.Settings.InspectorSettings;
using ConsoleApp.Settings.Rules;
using ConsoleApp.Settings.Senders;

namespace ConsoleApp.Settings
{
    public class AppSettings
    {
        public DataBaseSettings DataBase { get; set; }

        public JiraSettings Jira { get; set; }

        public IEnumerable<SenderSettings> Senders { get; set; }

        public IEnumerable<RuleSettings> Rules { get; set; }

        public StoryWorklogingSettings StoryWorklogingSettings { get; set; }

        public EmailSettings Email { get; set; }

        public BossSendExecutorSettings BossSendExecutor { get; set; }

        public TimerSettings Timer { get; set; }
    }
}
