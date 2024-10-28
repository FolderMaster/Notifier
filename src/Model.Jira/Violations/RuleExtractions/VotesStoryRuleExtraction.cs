using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public class VotesStoryRuleExtraction : IJiraRuleExtraction
    {
        public string Jql => "type = Story";

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
                    yield return new JiraUser(changeLog.Author);
                }
            }
        }
    }
}
