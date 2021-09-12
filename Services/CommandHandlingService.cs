using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private List<CommandModuleBase> _modules;
        
        public CommandHandlingService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _discord.InteractionCreated += Client_InteractionCreated;
            _discord.JoinedGuild += BuildModules;
            
            _modules = new List<CommandModuleBase>();
            foreach (var module in GetModules())
            {
                if (module != null)
                    _modules.Add(module);
            }
        }

        private IEnumerable<CommandModuleBase> GetModules()
        {
            IEnumerable<CommandModuleBase> modules = typeof(CommandModuleBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(CommandModuleBase)) && !t.IsAbstract)
                .Select(t => (CommandModuleBase)Activator.CreateInstance(t));
            return modules;
        }
        private async Task BuildModules(SocketGuild guild)
        {
            // Reflections are god tier
            IEnumerable<CommandModuleBase> modules = GetModules();
            
            foreach (CommandModuleBase module in modules)
            {
                if (module != null)
                    await module.Build(guild);
            }
        }
        
        // Generic command handler :D
        private async Task Client_InteractionCreated(SocketInteraction arg)
        {
            if(arg is SocketSlashCommand command)
            {
                foreach (CommandModuleBase module in _modules)
                {
                    if (module.Name == command.Data.Name)
                    {
                        await module.Execute(command);
                    }
                }
            }
        }
        
    }
}