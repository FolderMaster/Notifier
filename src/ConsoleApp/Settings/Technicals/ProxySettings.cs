using System.Net;

namespace ConsoleApp.Settings.Technicals
{
    public class ProxySettings
    {
        public string Url { get; set; }

        public CredentialsSettings? Credentials { get; set; }

        public IWebProxy CreateProxy() =>
            new WebProxy(Url, false, null, Credentials?.CreateCredentials());
    }
}
