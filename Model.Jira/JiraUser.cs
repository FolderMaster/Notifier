namespace Model.Jira
{
    public class JiraUser : IUser
    {
        private readonly string _username;

        public object Id => _username;

        public JiraUser(string username) => _username = username;
    }
}
