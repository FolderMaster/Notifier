using MimeKit;

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

        public MimeMessage CreateMimeMessage()
        {
            var result = new MimeMessage()
            {
                Subject = Subject,
                Body = new TextPart("plain")
                {
                    Text = Content.ToString()
                }
            };
            return result;
        }
    }
}
