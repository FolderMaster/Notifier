using Model.Jira.Violations.RuleExtractions;

namespace Model.Jira.Violations
{
    public class JiraViolation
    {
        public JiraUser User { get; private set; }

        public JiraIssue Issue { get; private set; }

        public JiraViolation(JiraUser user, JiraIssue issue)
        {
            User = user;
            Issue = issue;
        }
    }
}
