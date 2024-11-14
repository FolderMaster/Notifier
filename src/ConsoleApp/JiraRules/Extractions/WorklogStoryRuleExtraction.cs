using Atlassian.Jira;

using Model.Jira.Violations;

using JiraUser = Model.Jira.JiraUser;

namespace ConsoleApp.JiraRules.Extractions
{
    public class WorklogStoryRuleExtraction : IJiraRuleExtraction
    {
        public DateTime? StartDate { get; set; }

        public string Jql => $"type = story AND timespent > 0 {(StartDate != null ?
            $"AND worklogDate > '{StartDate?.ToString("yyyy-MM-dd")}'" : "")}";

        public async Task<IEnumerable<JiraUser>> FindViolators(Issue issue)
        {
            var result = new List<JiraUser>();
            var worklogs = await issue.GetWorklogsAsync();
            var usernames = new List<string>();
            foreach (var worklog in worklogs)
            {
                var username = worklog.AuthorUser.Username;
                if (worklog.TimeSpentInSeconds > 0 && !usernames.Contains(username))
                {
                    usernames.Add(username);
                    result.Add(new JiraUser(worklog.AuthorUser));
                }
            }
            return result;
        }
    }
}
