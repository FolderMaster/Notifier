using Model.Jira;

namespace ConsoleApp.Inspection
{
    public class InspectorController
    {
        private IEnumerable<IInspector> _inspectors;

        private ISenderContext _senderContext;

        public InspectorController(IEnumerable<IInspector> inspectors,
            ISenderContext senderContext)
        {
            _inspectors = inspectors;
            _senderContext = senderContext;
        }

        public async Task FindViolations()
        {
            /**var userDictionary = new Dictionary<JiraUser,
                Dictionary<IInspector, IEnumerable<JiraIssue>>>();
            foreach (var inspector in _inspectors)
            {
                var violations = await inspector.Inspect();
                var userIssueGroups = violations.GroupBy(v => v.User, v => v.Issue);
                foreach (var userIssueGroup in userIssueGroups)
                {
                    var user = userIssueGroup.Key;
                    if (!userDictionary.Keys.Contains(user))
                    {
                        userDictionary.Add(user,
                            new Dictionary<IInspector, IEnumerable<JiraIssue>>());
                    }
                    userDictionary[user].Add(inspector, userIssueGroup);
                }
            }
            
            foreach (var userGroup in userDictionary)
            {
                var content = "";
                foreach (var inspectorGroup in userGroup.Value)
                {
                    content += $"{inspectorGroup.Key}:\n";
                    foreach (var issue in inspectorGroup.Value)
                    {
                        content += $"\t- {issue.Link}\n";
                    }
                }
                _senderContext.Message.Content = content;
                await _senderContext.SendMessage(userGroup.Key);
            }**/

            var inspectorDictionary = new Dictionary<IInspector,
                Dictionary<JiraUser, IEnumerable<JiraIssue>>>();
            foreach (var inspector in _inspectors)
            {
                var violations = await inspector.Inspect();
                var userIssueDictionary = violations.GroupBy(v => v.User, v => v.Issue).
                    ToDictionary(p => p.Key, p => (IEnumerable<JiraIssue>)p);
                inspectorDictionary.Add(inspector, userIssueDictionary);
            }

            var content = "";
            foreach (var inspectorGroup in inspectorDictionary)
            {
                content += $"{inspectorGroup.Key}\n";
                foreach (var userGroup in inspectorGroup.Value)
                {
                    content += $"\t- {userGroup.Key.Id}:\n";
                    foreach (var issue in userGroup.Value)
                    {
                        content += $"\t\t- {issue.Link}\n";
                    }
                }
            }
            _senderContext.Message.Content = content;
            await _senderContext.SendMessage(new JiraUser("", "???"));
        }
    }
}
