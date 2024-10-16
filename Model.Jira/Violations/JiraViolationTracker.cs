using System.Reflection;

using Model.Jira.Violations.IssueRules;

namespace Model.Jira.Violations
{
    public class JiraViolationTracker
    {
        private Dictionary<string, IEnumerable<IIssueJiraRule>> _issueRulesGroups;

        private readonly JiraClient _client;

        public JiraViolationTracker(JiraClient client, IEnumerable<IIssueJiraRule>? rules = null)
        {
            _client = client;
            ReloadRules(rules);
        }

        public async IAsyncEnumerable<JiraViolation> FindViolations()
        {
            foreach (var issueRulesGroup in _issueRulesGroups)
            {
                var issues = await _client.GetIssuesFromJql(issueRulesGroup.Key);
                foreach (var issue in issues)
                {
                    foreach (var issueRule in issueRulesGroup.Value)
                    {
                        await foreach (var violator in issueRule.FindViolators(issue))
                        {
                            yield return new JiraViolation(issueRule, violator, issue);
                        }
                    }
                }
            }
        }

        public void ReloadRules(IEnumerable<IIssueJiraRule>? rules = null)
        {
            ArgumentNullException.ThrowIfNull(nameof(rules));
            _issueRulesGroups = rules.GroupBy(r => r.Jql).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }
}
