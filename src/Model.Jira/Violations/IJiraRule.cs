using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public interface IJiraRule
    {
        public string Jql { get; }

        public Task CheckIssues(IEnumerable<Issue> issues);
    }
}
