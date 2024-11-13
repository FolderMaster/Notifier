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
            await _executor.Execute(await GetViolations(issues));

        private async Task<IEnumerable<JiraViolation>> GetViolations(IEnumerable<Issue> issues)
        {
            var result = new List<JiraViolation>();
            foreach (var issue in issues)
            {
                foreach (var violator in await _extraction.FindViolators(issue))
                {
                    result.Add(new JiraViolation(violator, new JiraIssue(issue.JiraIdentifier,
                        $"{issue.Jira.Url}browse/{issue.Key.ToString()}")));
                }
            }
            return result;
        }
    }
}
