using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public class JiraViolationTracker
    {
        private Dictionary<string, IEnumerable<JiraRule>> _issueRulesGroups;

        private readonly JiraClient _client;

        public JiraViolationTracker(JiraClient client, IEnumerable<JiraRule> rules)
        {
            _client = client;
            ReloadRules(rules);
        }

        public async Task FindViolations()
        {
            foreach (var issueRulesGroup in _issueRulesGroups)
            {
                var issues = await _client.GetIssuesFromJql(issueRulesGroup.Key);
                foreach (var rule in issueRulesGroup.Value)
                {
                    await rule.Executor.Execute(GetViolationsForRule(issues, rule));
                }
            }
        }

        public void ReloadRules(IEnumerable<JiraRule> rules)
        {
            ArgumentNullException.ThrowIfNull(nameof(rules));

            _issueRulesGroups = rules.GroupBy(r => r.Extraction.Jql).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
        }

        private async IAsyncEnumerable<JiraViolation> GetViolationsForRule
            (IPagedQueryResult<Issue> issues, JiraRule rule)
        {
            foreach (var issue in issues)
            {
                await foreach (var violator in rule.Extraction.FindViolators(issue))
                {
                    yield return new JiraViolation(violator, new JiraIssue(issue.JiraIdentifier,
                        _client.CreateLink(issue.Key.ToString())));
                }
            }
        }
    }
}
