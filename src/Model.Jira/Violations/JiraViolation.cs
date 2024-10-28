namespace Model.Jira.Violations
{
    public record JiraViolation
    {
        public JiraUser User { get; private set; }

        public JiraIssue Issue { get; private set; }

        public JiraViolation(JiraUser user, JiraIssue issue)
        {
            User = user;
            Issue = issue;
        }
    }
}
