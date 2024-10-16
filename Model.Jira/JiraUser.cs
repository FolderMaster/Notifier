using User = Atlassian.Jira.JiraUser;

namespace Model.Jira
{
    public class JiraUser : IUser
    {
        public object Id { get; private set; }

        public string? Email { get; private set; }

        public JiraUser(string username, string? email = null)
        {
            Id = username;
            Email = email;
        }

        public JiraUser(User user) : this(user.Username, user.Email) { }
    }
}
