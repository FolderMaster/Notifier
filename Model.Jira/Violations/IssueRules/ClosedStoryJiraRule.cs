using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class ClosedStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "type = Story AND status = Closed";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            var subtasks = await issue.GetSubTasksAsync();
            foreach (var subtask in subtasks)
            {
                if (subtask.Type.Name != "Closed")
                {
                    var changeLogs = subtask.GetChangeLogsAsync().Result;
                    changeLogs = changeLogs.Reverse();
                    foreach (var changeLog in changeLogs)
                    {
                        if (changeLog.GetToValueField("Status") == "Closed")
                        {
                            yield return new JiraUser(changeLog.Author.Username,
                                changeLog.Author.Email);
                        }
                    }
                }
            }
        }
    }
}
