using User = Atlassian.Jira.JiraUser;

namespace Model.Jira
{
    public record JiraUser : IUser
    {
        public object Id { get; init; }

        public string? Email { get; init; }

        public JiraUser(string username, string? email = null)
        {
            Id = username;
            Email = email;
        }

        public JiraUser(User user) : this(user.Username, user.Email) { }
    }
}
