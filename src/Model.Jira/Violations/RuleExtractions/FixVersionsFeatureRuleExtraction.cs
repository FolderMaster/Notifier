using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public class FixVersionsFeatureRuleExtraction : IJiraRuleExtraction
    {
        public string Jql => "type = Feature AND fixVersion is EMPTY";

        public async IAsyncEnumerable<JiraUser> FindViolators(Issue issue)
        {
            yield return new JiraUser(issue.ReporterUser);
        }
    }
}
