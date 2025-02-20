using Hangfire;
using Hangfire.MemoryStorage;

using ConsoleApp.Inspection;
using ConsoleApp;
using ConsoleApp.Settings;

var settingsPath = "settings.json";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHangfire(c => c.UseMemoryStorage());
builder.Services.AddHangfireServer();

var app = builder.Build();
app.MapGet("/", () => Results.Redirect("/hangfire", true));
app.UseHangfireDashboard();

var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllBytes(settingsPath));
var host = NewConfigurator.RegisterServices(settings);

var inspectorController = host.Services.GetRequiredService<InspectorController>();
var timer = host.Services.GetRequiredService<ITimer>();

timer.TaskExpression = () => inspectorController.FindViolations();
timer.Start();

app.Run();
