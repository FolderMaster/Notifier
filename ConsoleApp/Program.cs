using Model;
using Model.Discord;

var token = "";

var bot = new DiscordBot(token);
bot.Log += Bot_Log;
bot.MessageReceived += Bot_MessageReceived;
bot.Start().GetAwaiter().GetResult();

async Task Bot_MessageReceived(object sender, MessageEventArgs args)
{
    if (args.User is DiscordUser discordUser)
    {
        if (!discordUser.IsBot)
        {
            await bot.SendMessage(new DiscordMessage("привет!"), args.User, args.Channel);
        }
    }
}

async Task Bot_Log(object sender, LogEventArgs args)
{
    Console.WriteLine(args.Content);
}