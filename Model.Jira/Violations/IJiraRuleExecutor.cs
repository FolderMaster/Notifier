namespace Model.Jira.Violations
{
    public interface IJiraRuleExecutor
    {
        Task Execute(JiraViolation violation);
    }
}
