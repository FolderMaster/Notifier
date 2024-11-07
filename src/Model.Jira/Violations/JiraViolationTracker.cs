namespace Model.Jira.Violations
{
    public class JiraViolationTracker
    {
        private Dictionary<string, IEnumerable<IJiraRule>> _issueRulesGroups;

        private readonly JiraClient _client;

        public JiraViolationTracker(JiraClient client, IEnumerable<IJiraRule> rules)
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
                    await rule.CheckIssues(issues);
                }
            }
        }

        public void ReloadRules(IEnumerable<IJiraRule> rules)
        {
            ArgumentNullException.ThrowIfNull(nameof(rules));

            _issueRulesGroups = rules.GroupBy(r => r.Jql).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }
}
