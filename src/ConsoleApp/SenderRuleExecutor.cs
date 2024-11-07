using Model.Email;
using Model.Jira.Violations;
using Model.Senders;

namespace ConsoleApp
{
    public class SenderRuleExecutor : IJiraRuleExecutor
    {
        private readonly IEnumerable<ISender> _senders;

        private readonly IMessage _message;

        public object? Content { get; set; }

        public SenderRuleExecutor(IEnumerable<ISender> senders, IMessage message)
        {
            ArgumentNullException.ThrowIfNull(senders, nameof(senders));
            ArgumentNullException.ThrowIfNull(message, nameof(message));

            _senders = senders;
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
                var content = $"{Content}\n";
                var violator = pair.Key;
                foreach (var violation in pair.Value)
                {
                    content += $"{violation.Issue.Link}\n";
                }
                _message.Content = content;
                foreach (var sender in _senders)
                {
                    await sender.SendMessage(_message, new EmailUser(violator.Email));
                }
            }
        }
    }
}
