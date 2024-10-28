using System.Net;

namespace ConsoleApp.Settings.Technicals
{
    public class CredentialsSettings
    {
        public string User { get; set; }

        public string Password { get; set; }

        public ICredentials CreateCredentials() => new NetworkCredential(User, Password);
    }
}
