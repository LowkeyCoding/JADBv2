using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DiscordBot.Modules
{
    public class CommandPing : CommandModuleBase
    {
        public CommandPing()
        {
            Name = "ping";
            Description = "returns pong";
        }
        
        public override async Task Build(SocketGuild guild)
        {
            var command = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description);

            try
            {
                await guild.CreateApplicationCommandAsync(command.Build());
            }
            catch (ApplicationCommandException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }

        public override async Task Execute(SocketSlashCommand command)
        {
            await command.RespondAsync($"pong");
        }
    }
}