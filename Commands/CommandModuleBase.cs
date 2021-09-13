using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Helpers;
using Newtonsoft.Json;

namespace DiscordBot.Commands
{
    public abstract class CommandModuleBase
    {
        public string Name;
        protected string Description;

        public virtual async Task Build(SocketGuild guild)
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

        public abstract Task Execute(Command command);
    }
}