using Atlassian.Jira;

using JiraRestClient = Atlassian.Jira.Jira;

namespace Model.Jira
{
    public class JiraClient : IUserCollector
    {
        private readonly JiraRestClient _jiraClient;

        public JiraClient(string url, string user, string password)
        {
            ArgumentNullException.ThrowIfNull(url, nameof(url));
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            _jiraClient = JiraRestClient.CreateRestClient(url, user, password);
        }

        public Task<bool> CheckUserId(object userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            try
            {
                _jiraClient.Users.GetUserAsync((string)userId).
                    GetAwaiter().GetResult();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<IPagedQueryResult<Issue>> GetIssuesFromJql(string jql,
            int? maxIssues = null) => _jiraClient.Issues.GetIssuesFromJqlAsync(jql, maxIssues);

        public Task<IDictionary<string, Issue>> GetIssues(IEnumerable<string> issueKeys) =>
            _jiraClient.Issues.GetIssuesAsync(issueKeys);

        public Task<Issue> GetIssue(string issueKey) => _jiraClient.Issues.GetIssueAsync(issueKey);
    }
}
