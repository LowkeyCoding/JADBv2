using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Helpers;

namespace DiscordBot.Services
{
    public class CommandService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly List<CommandModuleBase> _modules;
        
        public CommandService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            
            _discord.InteractionCreated += Client_InteractionCreated;
            _discord.JoinedGuild += BuildModules;
            
            _modules = new List<CommandModuleBase>();
        }

        public async Task IntializeAsync()
        {
            foreach (var module in GetModules())
            {
                if (module != null)
                {
                    _modules.Add(module);
                }
            }
        }

        private IEnumerable<CommandModuleBase> GetModules()
        {
            // Generics are fun :D
            IEnumerable<CommandModuleBase> modules = typeof(CommandModuleBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(CommandModuleBase)) && !t.IsAbstract)
                .Select(t =>
                {
                    // If we have a constructor that needs Services
                    ConstructorInfo[] constructors = t.GetConstructors();
                    if (constructors.Length > 0)
                    {
                        ConstructorInfo constructor = constructors[0];
                        ParameterInfo[] parameters = constructor.GetParameters();
                        object[] args = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                            args[i] = _services.GetService(parameters[i].ParameterType);
                        return (CommandModuleBase) Activator.CreateInstance(t, args);
                    }
                    // Such performance
                    return (CommandModuleBase) Activator.CreateInstance(t);
                });
            return modules;
        }
        public async Task BuildModules(SocketGuild guild)
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
                        await module.Execute(new Command(command));
                    }
                }
            }
        }
        
    }
}