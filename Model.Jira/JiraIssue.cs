namespace Model.Jira
{
    public class JiraIssue
    {
        public object Id { get; private set; }

        public JiraIssue(string id) => Id = id;
    }
}
