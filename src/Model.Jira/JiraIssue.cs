namespace Model.Jira
{
    public record JiraIssue
    {
        public object Id { get; private set; }

        public string Link { get; private set; }

        public JiraIssue(string id, string link)
        {
            Id = id;
            Link = link;
        }
    }
}
