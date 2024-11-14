using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Model.Jira.Violations;
using Model.Jira;
using Model.Senders;
using Model.Email;

using ConsoleApp.Settings;
using ConsoleApp.Settings.Rules;
using ConsoleApp.Settings.Senders;
using ConsoleApp.JiraRules;
using ConsoleApp.JiraRules.Executors;
using ConsoleApp.JiraRules.Extractions;

namespace ConsoleApp
{
    public static class Configurator
    {
        private static readonly Dictionary<SenderIdentifier, Type> _senders = new()
        {
            [SenderIdentifier.Email] = typeof(EmailSender)
        };

        private static readonly Dictionary<RuleIdentifier, Type> _rules = new()
        {
            [RuleIdentifier.WorklogStory] = typeof(WorklogStoryRule)
        };

        public static IHost RegisterServices(AppSettings settings)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((services) =>
            {
                RegisterSenders(services, settings.Senders);
                RegisterRules(services, settings.Rules);
                services.AddSingleton(s => new JiraClient(settings.Jira.Url, settings.Jira.User, 
                    settings.Jira.Password));
                services.AddSingleton<JiraViolationTracker>();
            }).Build();
            return host;
        }

        private static void RegisterSenders(IServiceCollection services,
            IEnumerable<SenderSettings> settingsSet)
        {
            RegisterModules<SenderIdentifier, ISender>(services, settingsSet, _senders);
        }

        private static void RegisterRules(IServiceCollection services,
            IEnumerable<RuleSettings> settingsSet)
        {
            services.AddSingleton<IMessage>((s) => new EmailMessage(""));
            services.AddSingleton<WorklogStoryRuleExtraction>();
            services.AddSingleton<SenderRuleExecutor>();

            RegisterModules<RuleIdentifier, IJiraRule>(services, settingsSet, _rules);
        }

        private static void RegisterModules<Key, Module>(IServiceCollection services,
            IEnumerable<ModuleSettings<Key>> settingsSet, Dictionary<Key, Type> types)
            where Key : notnull
            where Module : class
        {
            ArgumentNullException.ThrowIfNull(nameof(settingsSet));
            foreach (var settings in settingsSet)
            {
                var indentifier = settings.Identifier;
                if (types.TryGetValue(indentifier, out var type))
                {
                    if (!settings.Disabled)
                    {
                        if (settings.Properties == null)
                        {
                            services.AddSingleton<Module>((provider) =>
                                (Module)ActivatorUtilities.CreateInstance(provider, type));
                        }
                        else
                        {
                            services.AddSingleton<Module>((provider) =>
                            {
                                var module = ActivatorUtilities.CreateInstance(provider, type);
                                SetProperties(module, settings.Properties);
                                return (Module)module;
                            });
                        }
                    }
                }
            }
        }

        private static void SetProperties(object @object, IDictionary<string, object> properties)
        {
            foreach (var property in properties)
            {
                ReflectionService.SetProperty(@object, property.Key, property.Value);
            }
        }
    }
}
