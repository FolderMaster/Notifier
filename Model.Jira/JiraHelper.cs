using Atlassian.Jira;

namespace Model.Jira
{
    public static class JiraHelper
    {
        public static bool CheckStatus(this Issue issue, string status) =>
            issue.Status.Name == status;

        public static bool CheckType(this  Issue issue, string typeName) =>
            issue.Type.Name == typeName;

        public static async Task<IssueChangeLog> GetCreationChangeLog(this Issue issue) =>
            (await issue.GetChangeLogsAsync()).First();

        public static string? GetToValueField(this IssueChangeLog changeLog, string fieldName)
        {
            var item = changeLog.Items.FirstOrDefault(i => i.FieldName == fieldName);
            if (item == null)
            {
                return null;
            }
            return item.ToValue;
        }

        public static IEnumerable<string>? GetCustomField(this Issue issue, string fieldName)
        {
            var item = issue.CustomFields.FirstOrDefault(i => i.Name == fieldName);
            if (item == null)
            {
                return null;
            }
            return item.Values;
        }
    }
}
