using Atlassian.Jira;

namespace Model.Jira.Violations.IssueRules
{
    public class ClosedStoryJiraRule : IIssueJiraRule
    {
        public string Jql => "issuetype = Story AND status = Closed";

        public string Description => "";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue, JiraClient client)
        {
            var subtaskIds = issue.GetCustomField("Subtasks");
            var subtasks = await client.GetIssues(subtaskIds);
            foreach (var subtask in subtasks.Values)
            {
                if (!subtask.CheckStatus("Closed"))
                {
                    var changeLogs = subtask.GetChangeLogsAsync().Result;
                    changeLogs = changeLogs.Reverse();
                    foreach (var changeLog in changeLogs)
                    {
                        if (changeLog.GetToValueField("Status") == "Closed")
                        {
                            yield return new JiraUser(changeLog.Author.AccountId);
                        }
                    }
                }
            }
        }
    }
}
