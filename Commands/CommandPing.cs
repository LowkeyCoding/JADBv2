using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot.Helpers;

namespace DiscordBot.Commands
{
    public class CommandPing : CommandModuleBase
    {
        public CommandPing()
        {
            Name = "ping";
            Description = "returns pong";
        }

        public override async Task Execute(Command command)
        {
            await command.RespondAsync($"pong");
        }
    }
}