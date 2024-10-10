using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class VotesStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "type = Story";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            var changeLogs = await issue.GetChangeLogsAsync();
            changeLogs = changeLogs.Reverse();
            var usernames = new List<string>();
            foreach (var changeLog in changeLogs)
            {
                var username = changeLog.Author.Username;
                if ((changeLog.GetToValueField("Sprint") != null ||
                    changeLog.GetToValueField("status") == "In Progress") &&
                    changeLog.GetToValueField("Votes") == null &&
                    !username.Contains(username))
                {
                    usernames.Add(username);
                    yield return new JiraUser(username);
                }
            }
        }

        public override string ToString() => "VotesStoryJiraRule";
    }
}
