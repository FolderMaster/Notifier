using Atlassian.Jira;

using Model.Jira;
using Model.Jira.Violations;

using JiraUser = Model.Jira.JiraUser;

namespace ConsoleApp.JiraRules.Extractions
{
    public class VotesStoryRuleExtraction : IJiraRuleExtraction
    {
        public string Jql => "type = Story";

        public async Task<IEnumerable<JiraUser>> FindViolators(Issue issue)
        {
            var result = new List<JiraUser>();
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
                    result.Add(new JiraUser(changeLog.Author));
                }
            }
            return result;
        }
    }
}
