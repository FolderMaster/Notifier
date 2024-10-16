namespace Model.Jira
{
    public class JiraUser : IUser
    {
        public object Id { get; private set; }

        public JiraUser(string id) => Id = id;
    }
}
