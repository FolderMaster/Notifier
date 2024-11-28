using Model;
using Model.Email;
using Model.Jira;

namespace ConsoleApp.Inspection
{
    public class EmailSenderContext : BaseSenderContext
    {
        public EmailSenderContext(EmailSender sender, EmailMessage message) :
            base(sender, message) { }

        public override IUser ExtractUserForSending(JiraUser user) => new EmailUser(user.Email);
    }
}
