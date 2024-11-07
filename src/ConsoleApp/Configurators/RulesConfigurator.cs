using Microsoft.Extensions.DependencyInjection;

using ConsoleApp.Settings.Rules;

using Model.Jira.Violations;
using Model.Jira.Violations.RuleExtractions;
using Model.Email;
using Model.Senders;

namespace ConsoleApp.Configurators
{
    public class RulesConfigurator :
        ModulesConfigurator<RuleIdentifier, IJiraRule, RuleSettings>
    {
        protected override IDictionary<RuleIdentifier, Func<IServiceProvider, IJiraRule>>
            CreateServicesFunc() => new Dictionary<RuleIdentifier,
                Func<IServiceProvider, IJiraRule>>()
            {
                [RuleIdentifier.WorklogStory] = (s) => new JiraRule
                    (s.GetRequiredService<WorklogStoryRuleExtraction>(),
                    s.GetRequiredService<SenderRuleExecutor>())
            };

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IMessage>((s) => new EmailMessage(""));
            services.AddSingleton<WorklogStoryRuleExtraction>();
            services.AddSingleton<SenderRuleExecutor>();
        }

        protected override RuleIdentifier GetKeyFromSettings(RuleSettings settings) =>
            settings.Identifier;
    }
}
