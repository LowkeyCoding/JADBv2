using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Commands
{
    public class CommandUpdateBot : CommandModuleBase
    {
        private readonly CommandService _commandService;
        public CommandUpdateBot(CommandService commandService)
        {
            Name = "refresh";
            Description = "Updates the bot commands";
            _commandService = commandService;
        }

        public override async Task Execute(Command command)
        {
            if (command.GetUser() is IGuildUser guildUser)
            {
                await _commandService.BuildModules(guildUser.Guild as SocketGuild);
                await command.RespondAsync("Bot has been updated!");
                return;
            }

            await command.RespondAsync("Cannot update bot outside guild!");
        }
    }
}