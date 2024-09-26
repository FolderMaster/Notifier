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

            return _jiraClient.Users.GetUserAsync((string)userId).
                ContinueWith(task => task.Result != null);
        }
    }
}
