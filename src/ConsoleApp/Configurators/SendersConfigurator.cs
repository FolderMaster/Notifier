using ConsoleApp.Settings.Senders;

using Model.Email;
using Model.Senders;

namespace ConsoleApp.Configurators
{
    public class SendersConfigurator :
        ModulesConfigurator<SenderIdentifier, ISender, SenderSettings>
    {
        protected override IDictionary<SenderIdentifier, Func<IServiceProvider, ISender>>
            CreateServicesFunc() => new Dictionary<SenderIdentifier,
                Func<IServiceProvider, ISender>>()
            {
                [SenderIdentifier.Email] = (s) => new EmailSender()
            };

        protected override SenderIdentifier GetKeyFromSettings(SenderSettings settings) =>
            settings.Identifier;
    }
}
