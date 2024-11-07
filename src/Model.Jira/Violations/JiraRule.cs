using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public class JiraRule : IJiraRule
    {
        private readonly IJiraRuleExtraction _extraction;

        private readonly IJiraRuleExecutor _executor;

        public IJiraRuleExtraction Extraction => _extraction;

        public IJiraRuleExecutor Executor => _executor;

        public string Jql => _extraction.Jql;

        public JiraRule(IJiraRuleExtraction extraction, IJiraRuleExecutor executor)
        {
            ArgumentNullException.ThrowIfNull(extraction, nameof(extraction));
            ArgumentNullException.ThrowIfNull(executor, nameof(executor));

            _extraction = extraction;
            _executor = executor;
        }

        public async Task CheckIssues(IEnumerable<Issue> issues) =>
            await _executor.Execute(GetViolations(issues));

        private async IAsyncEnumerable<JiraViolation> GetViolations(IEnumerable<Issue> issues)
        {
            foreach (var issue in issues)
            {
                await foreach (var violator in _extraction.FindViolators(issue))
                {
                    yield return new JiraViolation(violator, new JiraIssue(issue.JiraIdentifier,
                        $"{issue.Jira.Url}browse/{issue.Key.ToString()}"));
                }
            }
        }
    }
}
