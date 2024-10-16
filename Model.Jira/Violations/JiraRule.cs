namespace Model.Jira.Violations
{
    public class JiraRule
    {
        public IJiraRuleExtraction Extraction { get; set; }

        public IJiraRuleExecutor Executor { get; set; }
    }
}
