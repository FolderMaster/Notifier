using ConsoleApp.Settings.Technicals;

namespace ConsoleApp.Settings
{
    public class DiscordSettings
    {
        public string Token { get; set; }

        public ProxySettings? Proxy { get; set; }
    }
}
