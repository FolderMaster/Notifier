using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class FixVersionsFeatureJiraRule : IIssueJiraRule
    {
        public string Jql => "type = Feature AND fixVersion is EMPTY";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            yield return new JiraUser(issue.Reporter);
        }

        public override string ToString() => "FixVersionsFeatureJiraRule";
    }
}
