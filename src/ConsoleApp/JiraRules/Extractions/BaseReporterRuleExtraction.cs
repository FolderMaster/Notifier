using Atlassian.Jira;

using Model.Jira.Violations;

using JiraUser = Model.Jira.JiraUser;

namespace ConsoleApp.JiraRules.Extractions
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
