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
                foreach (var issue in issues)
                {
                    foreach (var issueRule in issueRulesGroup.Value)
                    {
                        await foreach (var violator in issueRule.Extraction.FindViolators(issue))
                        {
                            await issueRule.Executor.Execute(new JiraViolation(violator,
                                new JiraIssue(issue.JiraIdentifier)));
                        }
                    }
                }
            }
        }

        public void ReloadRules(IEnumerable<JiraRule> rules)
        {
            ArgumentNullException.ThrowIfNull(nameof(rules));

            _issueRulesGroups = rules.GroupBy(r => r.Extraction.Jql).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }
}
