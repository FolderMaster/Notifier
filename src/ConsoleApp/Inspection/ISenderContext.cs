using Model.Jira;
using Model.Senders;

namespace ConsoleApp.Inspection
{
    public interface ISenderContext
    {
        public IMessage Message { get; }

        public Task SendMessage(JiraUser user);
    }
}
