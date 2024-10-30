using Model.Senders;

namespace ConsoleApp.Settings
{
    public class SenderSettings
    {
        public bool? Enabled { get; set; }

        public ISender Sender { get; set; }
    }
}
