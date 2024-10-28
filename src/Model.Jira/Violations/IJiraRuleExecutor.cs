namespace Model.Jira.Violations
{
    public interface IJiraRuleExecutor
    {
        public Task Execute(IAsyncEnumerable<JiraViolation> violations);
    }
}
