using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Jira;

using ConsoleApp.Settings;
using ConsoleApp.Inspection;
using Model.Email;

namespace ConsoleApp
{
    public static class NewConfigurator
    {
        public static IHost RegisterServices(AppSettings settings)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((services) =>
            {
                var jiraClient = new JiraClient(settings.Jira.Url, settings.Jira.User, settings.Jira.Password);
                var emailSender = new EmailSender()
                {
                    Email = settings.Email.Email,
                    Port = settings.Email.Port,
                    Url = settings.Email.Url,
                    Name = settings.Email.Name
                };
                var emailMessage = new EmailMessage("", settings.Email.Subject);
                services.AddSingleton(jiraClient);
                services.AddSingleton(emailSender);
                services.AddSingleton(emailMessage);
                services.AddSingleton<ISenderContext, EmailSenderContext>();

                // Регистрация инспекторов (они занимаются поиском нарушений в работе с Jira)
                RegisterInspectors(services, jiraClient, settings);

                services.AddSingleton<InspectorController>();
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
