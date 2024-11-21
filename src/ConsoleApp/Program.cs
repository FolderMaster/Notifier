using Microsoft.Extensions.DependencyInjection;

using Model.Jira.Violations;

using ConsoleApp;
using ConsoleApp.Settings;
using ConsoleApp.Inspection;

var settingsPath = "settings.json";

var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllBytes(settingsPath));

//var host = Configurator.RegisterServices(settings);
//var violationTracker = host.Services.GetRequiredService<JiraViolationTracker>();
//await violationTracker.FindViolations();


var host = NewConfigurator.RegisterServices(settings);

var inspectorController = host.Services.GetRequiredService<InspectorController>();
await inspectorController.FindViolations();

/**var dataBaseContext = new JsonDataBaseContext(settings.DataBase.FileName);
dataBaseContext.Load();

Console.WriteLine("Данные пользователей:");
Console.WriteLine("Index\tDiscordId\tJiraId\tEmailAddress");
for (var i = 0; i < dataBaseContext.UserData.Count; i++)
{
    var userData = dataBaseContext.UserData[i];
    Console.WriteLine($"{i}\t{userData.DiscordId}\t" +
        $"{userData.JiraId}\t{userData.EmailAddress}");
}**/

/**var emailSender = new EmailSender()
{
    Url = settings.Email.Url,
    Port = settings.Email.Port,
    Email = settings.Email.Email,
    Name = settings.Email.Name,
};**/

/**var discordBot = new DiscordBot(settings.Discord.Token, settings.Discord.Proxy?.CreateProxy());
discordBot.Log += Bot_Log;**/
//discordBot.MessageReceived += Bot_MessageReceived;

/**var commandsTasks = new Dictionary<ICommand, DiscordCommandDelegate>();
commandsTasks.Add(new DiscordCommand("help", "Describe commands.",
    new List<DiscordCommandOption>()
    { new DiscordCommandOption("command", "Described command.", typeof(string)) }),
    ExecuteHelpCommand);
commandsTasks.Add(new DiscordCommand("reg", "Register user.",
    new List<DiscordCommandOption>()
    {
        new DiscordCommandOption("jira", "Jira ID.", typeof(string), true),
        new DiscordCommandOption("email", "Email address.", typeof(string))
    }),
    ExecuteRegCommand);
var botCommandHelper = new DiscordBotCommandHelper(discordBot, commandsTasks);
discordBot.Ready += Bot_Ready;

discordBot.Start().GetAwaiter().GetResult();**/

/**Task Bot_MessageReceived(object sender, MessageEventArgs args)
{
    var content = $"Message received: author(ID:{args.User.Id}), channel(ID:{args.Channel.Id})" +
        $"\nContent:\n{args.Message.Content}";
    Console.WriteLine(content);
    return Task.CompletedTask;
}**/

/**async Task Bot_Ready(object sender, EventArgs args)
{
    while (true)
    {
        Console.WriteLine("Отправка сообщения.");
        Console.WriteLine("Введите контент сообщения:");
        var messageContent = Console.ReadLine();
        Console.WriteLine("Введите ID пользователя:");
        var userId = int.Parse(Console.ReadLine());
        if (SendMessageUser(userId, messageContent))
        {
            Console.WriteLine("Сообщение отправлено пользователю.");
        }
        else
        {
            Console.WriteLine("Ошибка при отправке сообщения пользователю!");
        }
    }
}

Task Bot_Log(object sender, LogEventArgs args)
{
    Console.WriteLine(args.Content);
    return Task.CompletedTask;
}

async Task ExecuteHelpCommand(DiscordCommandEventArgs args)
{
    if (!args.Options.Any())
    {
        var commandsDescriptions = new List<string>() { "Commands:" };
        foreach (var command in commandsTasks.Keys)
        {
            var commandDescription = $"/{command.Name}";
            if (command is DiscordCommand discordCommand)
            {
                commandDescription += $": {discordCommand.Description}";
            }
            commandsDescriptions.Add(commandDescription);
        }
        var content = string.Join("\n", commandsDescriptions);
        await args.SendMessage(new DiscordMessage(content));
    }
    else
    {
        var commandName = (string)args.Options["command"];
        var command = commandsTasks.Keys.FirstOrDefault(c => c.Name == commandName);
        if (command == null)
        {
            await args.SendMessage(new DiscordMessage($"Not found command {commandName}!"));
        }
        else
        {
            var commandDescriptions = new List<string>();
            if (command is DiscordCommand discordCommand)
            {
                commandDescriptions.Add($"Command {discordCommand.Name}: " +
                    $"{discordCommand.Description}");
                if (discordCommand.Options.Any())
                {
                    commandDescriptions.Add("Options:");
                    foreach (var option in discordCommand.Options)
                    {
                        commandDescriptions.Add($"-{option.Name}(type: {option.Type.Name}, " +
                            $"required: {option.IsRequired}): {option.Description}");
                    }
                }
            }
            else
            {
                commandDescriptions.Add($"Command {command.Name}");
            }
            var content = string.Join("\n", commandDescriptions);
            await args.SendMessage(new DiscordMessage(content));
        }
    }
}

async Task ExecuteRegCommand(DiscordCommandEventArgs args)
{
    var discordId = (ulong)args.User.Id;
    var jiraId = (string)args.Options["jira"];
    args.Options.TryGetValue("email", out var email);
    var emailAddress = (string?)email;

    var content = "";
    var embed = (DiscordEmbed?)null;
    if (RegisterUser(discordId, jiraId, emailAddress))
    {
        content = "Thanks for registration!";
        var embedFields = new List<DiscordEmbedField>()
        {
            new DiscordEmbedField("Discord ID", discordId),
            new DiscordEmbedField("Jira ID", jiraId),
        };
        embed = new DiscordEmbed("Properties", null, embedFields);
        if (emailAddress != null)
        {
            embedFields.Add(new DiscordEmbedField("Email address", emailAddress));
        }
    }
    else
    {
        content = "Error in registration!";
    }
    await args.SendMessage(new DiscordCommandMessage(content, true, embed));
}

bool RegisterUser(ulong discordId, string jiraId, string? emailAddress)
{
    //var isJiraChecked = jiraClient.CheckUserId(jiraId).Result;
    //var isEmailChecked = true;
    //if(emailAddress != null)
    //{
    //    isEmailChecked = emailSender.CheckUserId(jiraId).Result;
    //}
    //if (!isJiraChecked || !isEmailChecked)
    //{
    //    return false;
    //}
    var userData = dataBaseContext.UserData.FirstOrDefault((u) => u.DiscordId == discordId);
    if (userData == null)
    {
        userData = new UserData(discordId, emailAddress, jiraId);
        var index = dataBaseContext.UserData.Count;

        dataBaseContext.UserData.Add(userData);
        dataBaseContext.Save();

        Console.WriteLine("Регистрация:");
        Console.WriteLine("Index\tDiscordId\tJiraId\tEmailAddress");
        Console.WriteLine($"{index}\t{userData.DiscordId}\t" +
            $"{userData.JiraId}\t{userData.EmailAddress}");
    }
    else
    {
        var index = dataBaseContext.UserData.IndexOf(userData);
        userData.JiraId = jiraId;
        userData.EmailAddress = emailAddress;

        dataBaseContext.Save();

        Console.WriteLine("Обновленые данные:");
        Console.WriteLine("Index\tDiscordId\tJiraId\tEmailAddress");
        Console.WriteLine($"{index}\t{userData.DiscordId}\t" +
            $"{userData.JiraId}\t{userData.EmailAddress}");
    }
    
    return true;
}

bool SendMessageUser(int userIndex, string messageContent)
{
    if (userIndex >= dataBaseContext.UserData.Count)
    {
        return false;
    }
    var userData = dataBaseContext.UserData[userIndex];
    var discordTask = discordBot.SendMessage(new DiscordMessage(messageContent),
        new DiscordUser(userData.DiscordId, false));
    discordTask.Wait();
    //var emailTask = emailSender.SendMessage(new EmailMessage(messageContent),
    //    new EmailUser(userData.EmailAddress));
    //emailTask.Wait();
    return true
    // discordTask.IsCompletedSuccessfully **/ /** && emailTask.IsCompletedSuccessfully
    ;
}**/