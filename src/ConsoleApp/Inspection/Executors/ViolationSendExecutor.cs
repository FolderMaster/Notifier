using Model.Jira;

namespace ConsoleApp.Inspection.Executors
{
    internal class ViolationSendExecutor : IExecutor
    {
        private ISenderContext _senderContext;

        public ViolationSendExecutor(ISenderContext senderContext)
        {
            _senderContext = senderContext;
        }

        public async Task Execute(IEnumerable<InspectorViolation> violations)
        {
            var userDictionary = new Dictionary<JiraUser,
                Dictionary<IInspector, IEnumerable<JiraIssue>>>();
            foreach (var userGroup in violations.GroupBy(v => v.User))
            {
                var inspectorIssueDictionary = userGroup.GroupBy(v => v.Inspector, v => v.Issue).
                    ToDictionary(p => p.Key, p => (IEnumerable<JiraIssue>)p);
                userDictionary.Add(userGroup.Key, inspectorIssueDictionary);
            }

            foreach (var userGroup in userDictionary)
            {
                var content = "";
                foreach (var inspectorGroup in userGroup.Value)
                {
                    content += $"{inspectorGroup.Key}\n";
                    foreach (var issue in inspectorGroup.Value)
                    {
                        content += $"\t- {issue.Link}\n";
                    }
                }
                _senderContext.Message.Content = content;
                await _senderContext.SendMessage(userGroup.Key);
            }
        }
    }
}
