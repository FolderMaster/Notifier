using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Model;
using Model.Jira;
using Model.Email;

using ConsoleApp.Settings;
using ConsoleApp.Inspection;
using ConsoleApp.Inspection.SenderContexts;
using ConsoleApp.Inspection.Executors;
using ConsoleApp.Data;

namespace ConsoleApp
{
    public static class NewConfigurator
    {
        public static IHost RegisterServices(AppSettings settings)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((services) =>
            {
                var jiraClient = new JiraClient(settings.Jira.Url,
                    settings.Jira.User, settings.Jira.Password);
                var emailSender = new EmailSender()
                {
                    Email = settings.Email.Email,
                    Port = settings.Email.Port,
                    Url = settings.Email.Url,
                    Name = settings.Email.Name
                };
                var emailMessage = new EmailMessage("", settings.Email.Subject);
                var timer = new HangfireTimer<InspectorController>()
                {
                    Interval = settings.Timer.Interval
                };
                var bossUser = new EmailUser(settings.BossSendExecutor.Boss.ToString());
                var dataBaseContext = new
                    JsonDataBaseContext<DbInspectorViolation>(settings.DataBase.FileName);
                services.AddSingleton(jiraClient);
                services.AddSingleton(emailMessage);
                services.AddSingleton(emailSender);
                services.AddSingleton<IUser>(bossUser);
                services.AddSingleton<ITimer>(timer);
                services.AddSingleton<IDataBaseContext<DbInspectorViolation>>(dataBaseContext);
                services.AddSingleton<IFilter, SavedDataFilter>();
                services.AddSingleton<ISenderContext, EmailSenderContext>();
                services.AddSingleton<IExecutor, ViolationSendExecutor>();
                services.AddSingleton<IExecutor, BossSendExecutor>();

                // Регистрация инспекторов (они занимаются поиском нарушений в работе с Jira)
                RegisterInspectors(services, jiraClient, settings);

                services.AddSingleton<InspectorController>();
            }).Build();
            return host;
        }

        private static void RegisterInspectors(IServiceCollection services, JiraClient jiraClient,
            AppSettings settings)
        {
            if (settings.StoryWorklogingSettings.Enabled)
            {
                services.AddSingleton<IInspector>(new StoryWorklogingInspector(jiraClient,
                    settings.StoryWorklogingSettings));
            }
        }
    }
}
