using Model.Jira.Violations.IssueRules;

using ConsoleApp.Settings;

namespace ConsoleApp
{
    public static class ModuleConfigurator
    {
        public static IEnumerable<IIssueJiraRule> CreateModules
            (IEnumerable<ModuleSettings> settingsSet)
        {
            ArgumentNullException.ThrowIfNull(nameof(settingsSet));

            var result = new List<IIssueJiraRule>();
            foreach (var settings in settingsSet)
            {
                if (settings.Enabled != false)
                {
                    var type = Type.GetType(settings.Name);
                    var module = Activator.CreateInstance(type) as IIssueJiraRule;
                    if (settings.Properties != null && settings.Properties.Any())
                    {
                        var setProperties =  type.GetProperties().Where
                            ((p) => p.GetSetMethod() != null).ToDictionary
                            ((p) => p.Name, (p) => p.GetSetMethod());
                        foreach(var property in settings.Properties)
                        {
                            if (setProperties.TryGetValue(property.Key, out var getProperty))
                            {
                                getProperty.Invoke(module, new object[] { property.Value });
                            }
                        }
                    }
                    result.Add(module);
                }
            }
            return result;
        }
    }
}
