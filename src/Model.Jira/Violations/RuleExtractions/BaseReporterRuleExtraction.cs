using Atlassian.Jira;

namespace Model.Jira.Violations.RuleExtractions
{
    public abstract class BaseReporterRuleExtraction : IJiraRuleExtraction
    {
        public abstract string Jql { get; }

        public Task<IEnumerable<JiraUser>> FindViolators(Issue issue) =>
            Task.FromResult<IEnumerable<JiraUser>>(
                new List<JiraUser>()
                {
                    new JiraUser(issue.ReporterUser)
                });
    }
}
