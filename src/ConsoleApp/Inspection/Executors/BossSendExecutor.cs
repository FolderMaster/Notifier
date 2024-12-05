using Model;
using Model.Jira;

namespace ConsoleApp.Inspection.Executors
{
    public class BossSendExecutor : IExecutor
    {
        private ISenderContext _senderContext;

        private IUser _bossUser;

        public BossSendExecutor(ISenderContext senderContext, IUser bossUser)
        {
            _senderContext = senderContext;
            _bossUser = bossUser;
        }

        public async Task Execute(IEnumerable<InspectorViolation> violations)
        {
            var inspectorDictionary = new Dictionary<IInspector,
                Dictionary<JiraUser, IEnumerable<JiraIssue>>>();
            foreach (var inspectorGroup in violations.GroupBy(v => v.Inspector))
            {
                var userIssueDictionary = inspectorGroup.GroupBy(v => v.User, v => v.Issue).
                    ToDictionary(p => p.Key, p => (IEnumerable<JiraIssue>)p);
                inspectorDictionary.Add(inspectorGroup.Key, userIssueDictionary);
            }

            var content = "";
            foreach (var inspectorGroup in inspectorDictionary)
            {
                content += $"{inspectorGroup.Key}\n";
                foreach (var userGroup in inspectorGroup.Value)
                {
                    content += $"\t- {userGroup.Key.Id}\n";
                    foreach (var issue in userGroup.Value)
                    {
                        content += $"\t\t- {issue.Link}\n";
                    }
                }
            }
            _senderContext.Message.Content = content;
            await _senderContext.SendMessage(_bossUser);
        }
    }
}
