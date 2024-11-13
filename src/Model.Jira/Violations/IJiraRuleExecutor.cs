namespace Model.Jira.Violations
{
    public interface IJiraRuleExecutor
    {
        public Task Execute(IEnumerable<JiraViolation> violations);
    }
}
