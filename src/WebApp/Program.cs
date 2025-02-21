using Hangfire;
using Hangfire.MemoryStorage;

using ConsoleApp.Inspection;
using ConsoleApp;
using ConsoleApp.Settings;

var settingsPath = "settings.json";
var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllBytes(settingsPath));
var host = NewConfigurator.RegisterServices(settings);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(c => c.UseMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();
app.MapGet("/", () => Results.Redirect("/hangfire", true));
app.UseHangfireDashboard();
GlobalConfiguration.Configuration.UseActivator(new ServiceProviderJobActivator(host.Services));

var timer = host.Services.GetRequiredService<ITimer>() as HangfireTimer<InspectorController>;

timer.TaskExpression = (controller) => controller.FindViolations();
timer.Start();

app.Run();
