using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Jira;

using ConsoleApp.Settings;
using ConsoleApp.Inspection;

namespace ConsoleApp
{
    public static class NewConfigurator
    {
        public static IHost RegisterServices(AppSettings settings)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((services) =>
            {
                var jiraClient = new JiraClient(settings.Jira.Url, settings.Jira.User, settings.Jira.Password);
                services.AddSingleton(jiraClient);
                services.AddSingleton<InspectorController>();

                // Регистрация инспекторов (они занимаются поиском нарушений в работе с Jira)
                RegisterInspectors(services, jiraClient, settings);
            }).Build();
            return host;
        }

        private static void RegisterInspectors(IServiceCollection services, JiraClient jiraClient, AppSettings settings)
        {
            if (settings.StoryWorklogingSettings.Enabled)
            {
                services.AddSingleton<IInspector>(new StoryWorklogingInspector(jiraClient, settings.StoryWorklogingSettings));
            }
        }
    }
}
