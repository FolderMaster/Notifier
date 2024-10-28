using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public class RoughEstimationStoryRuleExtraction : IJiraRuleExtraction
    {
        public string Jql => "type = Story AND \"Rough estimation\" is EMPTY";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            yield return new JiraUser(issue.ReporterUser);
        }
    }
}
