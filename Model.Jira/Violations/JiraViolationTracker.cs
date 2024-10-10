using System.Reflection;

using Model.Jira.Violations.IssueRules;

namespace Model.Jira.Violations
{
    public class JiraViolationTracker
    {
        private Dictionary<string, IEnumerable<IIssueJiraRule>> _issueRulesGroups;

        private readonly JiraClient _jiraClient;

        public JiraViolationTracker(JiraClient jiraClient)
        {
            _jiraClient = jiraClient;
            ReloadRules();
        }

        public async IAsyncEnumerable<JiraViolation> FindViolations()
        {
            foreach (var issueRulesGroup in _issueRulesGroups)
            {
                var issues = await _jiraClient.GetIssuesFromJql(issueRulesGroup.Key);
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

        public void ReloadRules()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().
                Where(t => !t.IsAbstract && typeof(IIssueJiraRule).IsAssignableFrom(t));
            var rules = new List<IIssueJiraRule>();
            foreach (var type in types)
            {
                rules.Add(Activator.CreateInstance(type) as IIssueJiraRule);
            }
            _issueRulesGroups = rules.GroupBy(r => r.Jql).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
        }
    }
}
