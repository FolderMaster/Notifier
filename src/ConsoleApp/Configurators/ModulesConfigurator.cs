using Microsoft.Extensions.DependencyInjection;

using ConsoleApp.Settings;

namespace ConsoleApp.Configurators
{
    public abstract class ModulesConfigurator<Key, Module, Settings> where Key : notnull
        where Module : class
        where Settings : ModuleSettings
    {
        private readonly IDictionary<Key, Func<IServiceProvider, Module>> _sendersFunc;

        public ModulesConfigurator()
        {
            _sendersFunc = CreateServicesFunc();
        }

        protected virtual void RegisterServices(IServiceCollection services) { }

        protected abstract IDictionary<Key, Func<IServiceProvider, Module>> CreateServicesFunc();

        protected abstract Key GetKeyFromSettings(Settings settings);

        public void RegisterModules(IEnumerable<Settings> settingsSet, IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(nameof(settingsSet));

            RegisterServices(services);
            var rulesFunc = new Dictionary<Key, Func<IServiceProvider, Module>>(_sendersFunc);
            foreach (var settings in settingsSet)
            {
                var indentifier = GetKeyFromSettings(settings);
                if (rulesFunc.TryGetValue(indentifier, out var ruleFunc))
                {
                    if (settings.Disabled)
                    {
                        rulesFunc.Remove(indentifier);
                    }
                    else if (settings.Properties != null)
                    {
                        rulesFunc[indentifier] = (provider) =>
                        {
                            var rule = ruleFunc(provider);
                            ReflectionService.SetProperties(rule, settings.Properties);
                            return rule;
                        };
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(settings));
                }
            }
            foreach (var ruleFunc in rulesFunc)
            {
                services.AddSingleton<Module>(ruleFunc.Value);
            }
        }
    }
}
