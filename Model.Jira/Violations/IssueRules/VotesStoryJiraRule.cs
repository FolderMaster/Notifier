using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class VotesStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "issuetype = Story";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue, JiraClient client)
        {
            var changeLogs = await issue.GetChangeLogsAsync();
            changeLogs = changeLogs.Reverse();
            foreach (var changeLog in changeLogs)
            {
                if ((changeLog.GetToValueField("Sprint") != null ||
                    changeLog.GetToValueField("Status") == "In Progress") &&
                    changeLog.GetToValueField("Votes") == null)
                {
                    yield return new JiraUser(changeLog.Author.AccountId);
                }
            }
        }
    }
}
