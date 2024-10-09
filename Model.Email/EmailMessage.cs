using Model.Senders;

namespace Model.Email
{
    public class EmailMessage : IMessage
    {
        public object Content { get; }

        public string? Subject { get; }

        public EmailMessage(object content, string? subject = null)
        {
            Content = content;
            Subject = subject;
        }
    }
}
