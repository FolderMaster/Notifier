using Model.Jira.Violations.IssueRules;

using Atlassian.Jira;

namespace Model.Jira.Violations
{
    public class JiraViolation
    {
        public IIssueJiraRule Rule { get; private set; }

        public JiraUser User { get; private set; }

        public Issue Issue { get; private set; }

        public JiraViolation(IIssueJiraRule rule, JiraUser user, Issue issue)
        {
            Rule = rule;
            User = user;
            Issue = issue;
        }
    }
}
