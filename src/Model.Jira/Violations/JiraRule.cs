namespace Model.Jira.Violations
{
    public class JiraRule
    {
        public IJiraRuleExtraction Extraction { get; private set; }

        public IJiraRuleExecutor Executor { get; private set; }

        public JiraRule(IJiraRuleExtraction extraction, IJiraRuleExecutor executor)
        {
            Extraction = extraction;
            Executor = executor;
        }
    }
}
