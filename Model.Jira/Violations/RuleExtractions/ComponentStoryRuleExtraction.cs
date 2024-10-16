using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public class ComponentStoryRuleExtraction : IJiraRuleExtraction
    {
        public string Jql => "type = Story AND component is EMPTY";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            yield return new JiraUser(issue.ReporterUser);
        }
    }
}
