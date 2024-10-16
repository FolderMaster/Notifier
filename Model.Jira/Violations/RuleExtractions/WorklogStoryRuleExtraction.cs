using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public class WorklogStoryRuleExtraction : IJiraRuleExtraction
    {
        public DateTime? StartDate { get; set; }

        public string Jql => $"type = story AND timespent > 0 {(StartDate != null ?
            $"AND worklogDate > '{StartDate?.ToString("yyyy-MM-dd")}'" : "")}";

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
                    yield return new JiraUser(worklog.AuthorUser);
                }
            }
        }
    }
}
