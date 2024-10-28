using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public interface IJiraRuleExtraction
    {
        public string Jql { get; }

        public IAsyncEnumerable<JiraUser> FindViolators(Issue issue);
    }
}
