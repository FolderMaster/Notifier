using Model.Jira.Violations;

namespace ConsoleApp.Settings
{
    public class RuleSettings
    {
        public bool? Enabled { get; set; }

        public IJiraRuleExtraction Extraction { get; set; }

        public IJiraRuleExecutor Executor { get; set; }
    }
}
