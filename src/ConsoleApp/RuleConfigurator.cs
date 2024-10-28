using ConsoleApp.Settings;

using Model.Jira.Violations;

namespace ConsoleApp
{
    public static class RuleConfigurator
    {
        public static IEnumerable<JiraRule> CreateModules
            (IEnumerable<RuleSettings> settingsSet)
        {
            ArgumentNullException.ThrowIfNull(nameof(settingsSet));

            var result = new List<JiraRule>();
            foreach (var settings in settingsSet)
            {
                if (settings.Enabled != false)
                {
                    result.Add(new JiraRule(settings.Extraction, settings.Executor));
                }
            }
            return result;
        }
    }
}
