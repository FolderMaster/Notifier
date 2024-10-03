using System.Net;

namespace ConsoleApp.Settings
{
    public class ProxySettings
    {
        public string Url { get; set; }

        public Credentials? Credentials { get; set; }

        public IWebProxy GetProxy()
        {
            if(Credentials != null)
            {
                var credentials = new NetworkCredential(Credentials.User, Credentials.Password);
                return new WebProxy(Url, false, null, credentials);
            }
            return new WebProxy(Url, false, null);
        }
    }
}
