using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public interface IIssueJiraRule
    {
        public string Jql { get; }

        public IAsyncEnumerable<JiraUser> FindViolators(Issue issue);
    }
}
