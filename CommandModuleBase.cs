using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public abstract class CommandModuleBase
    {
        public string Name;
        public string Description;

        public abstract Task Build(SocketGuild guild);

        public abstract Task Execute(SocketSlashCommand command);
    }
}