using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public interface IIssueJiraRule
    {
        public string Jql { get; }

        public string Description { get; }

        public IAsyncEnumerable<JiraUser> FindViolators(Issue issue, JiraClient client);
    }
}
