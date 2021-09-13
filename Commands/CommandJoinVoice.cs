using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Helpers;
using DiscordBot.Services;

namespace DiscordBot.Commands
{
    public class CommandJoinVoice : CommandModuleBase
    {
        private readonly AudioService _audioService;
        public CommandJoinVoice(AudioService audioService)
        {
            Name = "join";
            Description = "Bot will join the voice chat the user is connected to if any";
            _audioService = audioService;
        }

        public override Task Execute(Command command)
        {

            IVoiceChannel channel = command.GetVoiceChannel();
            if (channel == null)
            {
                command.RespondAsync($"User must be inside voice channel to use join command.", ephemeral: true);
            }
            ConnectChannel(channel);
            command.RespondAsync("Joined voice chat", ephemeral: true);
            return Task.CompletedTask;
        }

        private async Task ConnectChannel(IVoiceChannel channel)
        {
            _audioService.Client = await channel.ConnectAsync();
        }
    }
}