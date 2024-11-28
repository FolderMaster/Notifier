using ConsoleApp.Settings.InspectorSettings;
using Model.Jira;
using Model.Jira.Violations;
using JiraUser = Model.Jira.JiraUser;

namespace ConsoleApp.Inspection
{
    public class StoryWorklogingInspector : IInspector
    {
        private readonly JiraClient _client;
        private DateTime? _startDate;
        private string _jql = $"type = story AND timespent > 0";

        private string _message;

        public StoryWorklogingInspector(JiraClient client, StoryWorklogingSettings settings)
        {
            _client = client;
            _startDate = settings.InspectionStartDate;

            _message = settings.ViolationMessage;

            if (settings.InspectionStartDate != null)
            {
                _jql += $" AND worklogDate > '{_startDate?.ToString("yyyy-MM-dd")}'";
            }
        }

        public async Task<IEnumerable<JiraViolation>> Inspect()
        {
            var issues = await _client.GetIssuesFromJql(_jql);

            var violations = new List<JiraViolation>();
            foreach (var issue in issues)
            {
                var worklogs = await issue.GetWorklogsAsync();
                var usernames = new List<string>();
                foreach (var worklog in worklogs)
                {
                    var username = worklog.AuthorUser.Username;
                    if (worklog.TimeSpentInSeconds > 0 && !usernames.Contains(username))
                    {
                        usernames.Add(username);

                        var jiraUser = new JiraUser(worklog.AuthorUser);
                        var issueLink = $"{issue.Jira.Url}browse/{issue.Key.ToString()}";
                        violations.Add(new JiraViolation(jiraUser, new JiraIssue(issue.JiraIdentifier, issueLink)));
                    }

                }
            }

            return violations;
        }

        public override string ToString() => _message;
    }
}
