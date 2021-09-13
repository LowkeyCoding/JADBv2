using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Services;

namespace DiscordBot.Commands
{
    public class CommandLeave : CommandModuleBase
    {
        private readonly AudioService _audioService;
        private readonly DiscordSocketClient _discord;
        
        public CommandLeave(AudioService audioService, DiscordSocketClient discord)
        {
            Name = "leave";
            Description = "Leaves current voicechat";
            _audioService = audioService;
            _discord = discord;
        }
        public override Task Execute(Command command)
        {
            IVoiceChannel channel = command.GetVoiceChannel();
            if (channel == null)
            {
                command.RespondAsync($"User must be inside voice channel to use leave command.", ephemeral: true);
                return Task.CompletedTask;
            }

            Disconnect(channel);
            command.RespondAsync($"Disconnected from voicechat", ephemeral: true);
            return Task.CompletedTask;
        }

        private async Task Disconnect(IVoiceChannel channel)
        {
            await _audioService.Client.StopAsync();
            await channel.DisconnectAsync();
        }
    }
}