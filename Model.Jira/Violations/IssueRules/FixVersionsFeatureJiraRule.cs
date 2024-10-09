using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class FixVersionsFeatureJiraRule : IIssueJiraRule
    {
        public string Jql => "issuetype = Feature";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue, JiraClient client)
        {
            var changeLog = await issue.GetCreationChangeLog();
            if (changeLog.GetToValueField("fix Versions") == null)
            {
                yield return new JiraUser(changeLog.Author.AccountId);
            }
        }
    }
}
