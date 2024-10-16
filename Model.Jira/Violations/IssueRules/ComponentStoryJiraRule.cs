using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class ComponentStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "type = Story AND component is EMPTY";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            yield return new JiraUser(issue.Reporter);
        }

        public override string ToString() => "ComponentStoryJiraRule";
    }
}
