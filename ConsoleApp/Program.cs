using Model;
using Model.Discord;

var token = "";

var bot = new DiscordBot(token);
bot.Log += Bot_Log;
bot.MessageReceived += Bot_MessageReceived;

var commandsTasks = new Dictionary<ICommand, DiscordCommandDelegate>();
commandsTasks.Add(new DiscordCommand("help", "Describe commands.",
    [new DiscordCommandOption("command", "Described command.", typeof(string), false)]),
    ExecuteHelpCommand);
commandsTasks.Add(new DiscordCommand("reg", "Register user.",
    [new DiscordCommandOption("jira", "Jira ID.", typeof(long), true),
    new DiscordCommandOption("email", "Email address.", typeof(string), true)]),
    ExecuteRegCommand);

var botCommandHelper = new DiscordBotCommandHelper(bot, commandsTasks);
bot.Start().GetAwaiter().GetResult();

async Task Bot_MessageReceived(object sender, MessageEventArgs args)
{
    if (args.User is DiscordUser discordUser)
    {
        if (!discordUser.IsBot)
        {
            await bot.SendMessage(new DiscordMessage("Привет!"), args.User, args.Channel);
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
    var discordId = args.User.Id;
    var jiraId = (long)args.Options["jira"];
    var emailAddress = (string)args.Options["email"];
    var content = "Thanks for registration! Your parameters:\n" +
        $"Discord ID: {discordId}\n" +
        $"Jira ID: {jiraId}\n" +
        $"Email address: {emailAddress}";
    await args.SendMessage(new DiscordBotMessage(content, true));

}