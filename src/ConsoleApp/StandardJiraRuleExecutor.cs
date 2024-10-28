using Model;
using Model.Email;
using Model.Jira.Violations;
using Model.Senders;

namespace ConsoleApp
{
    public class StandardJiraRuleExecutor : IJiraRuleExecutor
    {
        public static ISender Sender { get; set; }

        public IMessage Message { get; set; }

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
                Message.Content = content;
                // !!!!!!!!!!!!!!!!!!!!!!!
                if (violator.Email == "")
                {
                    await Sender.SendMessage(Message, new EmailUser(violator.Email));
                }
            }
        }
    }
}
