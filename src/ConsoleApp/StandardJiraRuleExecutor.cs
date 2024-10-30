using Model.Email;
using Model.Jira.Violations;
using Model.Senders;

namespace ConsoleApp
{
    public class StandardJiraRuleExecutor : IJiraRuleExecutor
    {
        private readonly ISender _sender;

        private readonly IMessage _message;

        public StandardJiraRuleExecutor(ISender sender, IMessage message)
        {
            _sender = sender;
            _message = message;
        }

        public async Task Execute(IAsyncEnumerable<JiraViolation> violations)
        {
            var violationsList = new List<JiraViolation>();
            await foreach(JiraViolation violation in violations)
            {
                violationsList.Add(violation);
            }
            
            var violatorsDictionary = violationsList.GroupBy(v => v.User).
                ToDictionary(g => g.Key, g => g.AsEnumerable());
            foreach (var pair in violatorsDictionary)
            {
                var content = "Кажется, время залогировано в Story, проверь пожалуйста:\n";
                var violator = pair.Key;
                foreach (var violation in pair.Value)
                {
                    content += $"{violation.Issue.Link}\n";
                }
                _message.Content = content;
                // !!!!!!!!!!!!!!!!!!!!!!!
                if (violator.Email == "")
                {
                    await _sender.SendMessage(_message, new EmailUser(violator.Email));
                }
            }
        }
    }
}
