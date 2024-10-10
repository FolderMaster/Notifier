using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class WorklogStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "type = story AND timespent > 0 AND worklogDate > '2024/10/01'";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            var worklogs = await issue.GetWorklogsAsync();
            var usernames = new List<string>();
            foreach (var worklog in worklogs)
            {
                var username = worklog.AuthorUser.Username;
                if (worklog.TimeSpentInSeconds > 0 && !usernames.Contains(username))
                {
                    usernames.Add(username);
                    yield return new JiraUser(username);
                }
            }
        }

        public override string ToString() => "WorklogStoryJiraRule";
    }
}
