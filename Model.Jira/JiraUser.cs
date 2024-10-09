namespace Model.Jira
{
    public class JiraUser : IUser
    {
        private readonly string _id;

        public object Id => _id;

        public JiraUser(string id) => _id = id;
    }
}
