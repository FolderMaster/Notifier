using MailKit.Net.Smtp;
using MimeKit;

using Model.Senders;

namespace Model.Email
{
    public class EmailSender : IMultiSender
    {
        public string Url { get; set; }

        public int Port { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IMessage? CheckUserMessage { get; set; }

        public async Task<bool> CheckUserId(object userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            try
            {
                await SendMessage(CheckUserMessage, new EmailUser((string)userId));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SendMessage(IMessage message, IUser user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            await SendMessage(message, new IUser[] { user });
        }

        public async Task SendMessage(IMessage message, IEnumerable<IUser> users)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(users, nameof(users));

            MimeMessage mimeMessage;
            if (message is EmailMessage emailMessage)
            {
                mimeMessage = emailMessage.CreateMimeMessage();
            }
            else
            {
                mimeMessage = new MimeMessage()
                {
                    Body = new TextPart("plain")
                    {
                        Text = message.Content.ToString()
                    }
                };
            }
            mimeMessage.From.Add(new MailboxAddress(Name, Email));
            foreach (var user in users)
            {
                mimeMessage.To.Add(MailboxAddress.Parse(user.Id.ToString()));
            }

            using (var client = new SmtpClient())
            {
                client.Connect(Url, Port);
                await client.SendAsync(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
}
