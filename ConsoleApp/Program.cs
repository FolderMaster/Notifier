using Microsoft.Extensions.Configuration;

using Model;
using Model.Discord;
using Model.Email;
using Model.Jira;

using ConsoleApp.Settings;
using ConsoleApp.Data;

var configuration = new ConfigurationBuilder().AddJsonFile("settings.json").Build();
var settings = configuration.Get<Settings>();

var dataBaseContext = new DataBaseContext(settings.DataBase.ConnectionString);
dataBaseContext.Database.EnsureCreated();

Console.WriteLine("Данные пользователей:");
Console.WriteLine("Id\tDiscordId\tJiraId\tEmailAddress");
foreach (var userData in dataBaseContext.UserData)
{
    Console.WriteLine($"{userData.Id}\t{userData.DiscordId}\t" +
        $"{userData.JiraId}\t{userData.EmailAddress}");
}

//var emailSender = new EmailSender(settings.Email.Url, settings.Email.Port);
//var jiraClient = new JiraClient(settings.Jira.Url, settings.Jira.Url, settings.Jira.Url);

var discordBot = new DiscordBot(settings.Discord.Token);
//bot.Log += Bot_Log;
//discordBot.MessageReceived += Bot_MessageReceived;

var commandsTasks = new Dictionary<ICommand, DiscordCommandDelegate>();
commandsTasks.Add(new DiscordCommand("help", "Describe commands.",
    [new DiscordCommandOption("command", "Described command.", typeof(string))]),
    ExecuteHelpCommand);
commandsTasks.Add(new DiscordCommand("reg", "Register user.",
    [new DiscordCommandOption("jira", "Jira ID.", typeof(string), true),
    new DiscordCommandOption("email", "Email address.", typeof(string))]),
    ExecuteRegCommand);
var botCommandHelper = new DiscordBotCommandHelper(discordBot, commandsTasks);
discordBot.Ready += Bot_Ready;

discordBot.Start().GetAwaiter().GetResult();

/**Task Bot_MessageReceived(object sender, MessageEventArgs args)
{
    var content = $"Message received: author(ID:{args.User.Id}), channel(ID:{args.Channel.Id})" +
        $"\nContent:\n{args.Message.Content}";
    Console.WriteLine(content);
    return Task.CompletedTask;
}**/

async Task Bot_Ready(object sender, EventArgs args)
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

/**Task Bot_Log(object sender, LogEventArgs args)
{
    Console.WriteLine(args.Content);
    return Task.CompletedTask;
}**/

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
    if (RegisterUser(discordId, jiraId, emailAddress))
    {
        content = "Thanks for registration! Your parameters:\n" +
            $"Discord ID: {discordId}\n" +
            $"Jira ID: {jiraId}\n" +
            (emailAddress != null ? $"Email address: {emailAddress}" : "");
    }
    else
    {
        content = "Error in registration!";
    }
    await args.SendMessage(new DiscordBotMessage(content, true));
}

bool RegisterUser(ulong discordId, string jiraId, string? emailAddress)
{
    /**var isJiraChecked = jiraClient.CheckUserId(jiraId).Result;
    var isEmailChecked = true;
    if(emailAddress != null)
    {
        isEmailChecked = emailSender.CheckUserId(jiraId).Result;
    }
    if (!isJiraChecked || !isEmailChecked)
    {
        return false;
    }**/
    var userData = dataBaseContext.UserData.FirstOrDefault((u) => u.DiscordId == discordId);
    if (userData == null)
    {
        dataBaseContext.UserData.Add(new UserData(discordId, emailAddress, jiraId));
        dataBaseContext.SaveChanges();

        Console.WriteLine("Регистрация:");
        Console.WriteLine("Id\tDiscordId\tJiraId\tEmailAddress");
        Console.WriteLine($"{userData.Id}\t{userData.DiscordId}\t" +
            $"{userData.JiraId}\t{userData.EmailAddress}");
    }
    else
    {
        userData.JiraId = jiraId;
        userData.EmailAddress = emailAddress;
        dataBaseContext.SaveChanges();

        Console.WriteLine("Обновлены данные:");
        Console.WriteLine("Id\tDiscordId\tJiraId\tEmailAddress");
        Console.WriteLine($"{userData.Id}\t{userData.DiscordId}\t" +
            $"{userData.JiraId}\t{userData.EmailAddress}");
    }
    
    return true;
}

bool SendMessageUser(int userId, string messageContent)
{
    var userData = dataBaseContext.UserData.FirstOrDefault((u) => u.Id == userId);
    if (userData == null)
    {
        return false;
    }
    var discordTask = discordBot.SendMessage(new DiscordMessage(messageContent),
        new DiscordUser(userData.DiscordId, false));
    discordTask.Wait();
    /**var emailTask = emailSender.SendMessage(new EmailMessage(messageContent),
        new EmailUser(userData.EmailAddress));
    emailTask.Wait();**/
    return discordTask.IsCompletedSuccessfully /** && emailTask.IsCompletedSuccessfully**/;
}