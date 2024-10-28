using Atlassian.Jira;

namespace Model.Jira
{
    public static class JiraHelper
    {
        public static async Task<object?> GetCreationField(this Issue issue,
            string fieldName, object? fieldValue = null)
        {
            var changeLogs = await issue.GetChangeLogsAsync();
            var result = fieldValue ?? issue.GetCustomField(fieldName);
            if (changeLogs.Any())
            {
                changeLogs = changeLogs.Reverse();
                foreach (var changeLog in changeLogs)
                {
                    result = changeLog.GetFromValueField(fieldName);
                }
            }
            return result;
        }

        public static string? GetToValueField(this IssueChangeLog changeLog, string fieldName)
        {
            var item = changeLog.Items.FirstOrDefault(i => i.FieldName == fieldName);
            if (item == null)
            {
                return null;
            }
            return item.ToValue;
        }

        public static string? GetFromValueField(this IssueChangeLog changeLog, string fieldName)
        {
            var item = changeLog.Items.FirstOrDefault(i => i.FieldName == fieldName);
            if (item == null)
            {
                return null;
            }
            return item.FromValue;
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
