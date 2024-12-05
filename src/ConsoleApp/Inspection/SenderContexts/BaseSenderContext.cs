using Model;
using Model.Jira;
using Model.Senders;

namespace ConsoleApp.Inspection.SenderContexts
{
    public abstract class BaseSenderContext : ISenderContext
    {
        private ISender _sender;

        public IMessage Message { get; private set; }

        public BaseSenderContext(ISender sender, IMessage message)
        {
            _sender = sender;
            Message = message;
        }

        public async Task SendMessage(JiraUser user) =>
            await _sender.SendMessage(Message, ExtractUserForSending(user));

        public async Task SendMessage(IUser user) =>
            await _sender.SendMessage(Message, user);

        public abstract IUser ExtractUserForSending(JiraUser user);
    }
}
