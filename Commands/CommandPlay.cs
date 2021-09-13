using System;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DiscordBot.Commands
{
    public class CommandPlay : CommandModuleBase
    {
        private readonly AudioService _audioService;
        public CommandPlay(AudioService audioService)
        {
            Name = "play";
            Description = "Bot will join the voice chat the user is connected to if any";
            _audioService = audioService;
        }

        public override async Task Build(SocketGuild guild)
        {
            var command = new SlashCommandBuilder()
                .WithName(Name)
                .WithDescription(Description)
                .AddOption(
                    "url", 
                    ApplicationCommandOptionType.String, 
                    "The url to song",
                    true
                );
                
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

        public override Task Execute(Command command)
        {
            Play(command);
            command.RespondAsync("playing.", ephemeral: true);
            return Task.CompletedTask;
        }

        private async Task Play(Command command)
        {
            await _audioService.SendAsync("C:/Users/Lowkey/Downloads/chill.mp3");
        }
    }
}