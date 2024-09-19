using System.Net.Mail;

namespace Model.Email
{
    public class EmailSender : ISender
    {
        private SmtpClient _client;

        public EmailSender(string? host, int port)
        {
            _client = new SmtpClient(host, port);
        }

        public async Task SendMessage(IMessage message, IUser user)
        {
            ArgumentNullException.ThrowIfNull(message, nameof(message));
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var mailMessage = new MailMessage()
            {
                Body = message.Content.ToString()
            };
            mailMessage.To.Add(user.Id.ToString());
            if (message is EmailMessage messageEmail)
            {
                mailMessage.Subject = messageEmail.Subject;
            }

            await _client.SendMailAsync(mailMessage);
        }
    }
}
