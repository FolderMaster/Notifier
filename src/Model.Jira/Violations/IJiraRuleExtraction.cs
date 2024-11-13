using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public interface IJiraRuleExtraction
    {
        public string Jql { get; }

        public Task<IEnumerable<JiraUser>> FindViolators(Issue issue);
    }
}
