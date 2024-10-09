using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class WorklogStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "issuetype = Story";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue, JiraClient client)
        {
            var result = new List<string>();
            var worklogs = await issue.GetWorklogsAsync();
            foreach (var worklog in worklogs)
            {
                if (worklog.TimeSpentInSeconds != 0)
                {
                    yield return new JiraUser(worklog.AuthorUser.AccountId);
                }
            }
        }
    }
}
