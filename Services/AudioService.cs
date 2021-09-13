using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Audio;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        public Process Ffmpeg;

        public IAudioClient Client { get; set; }

        public AudioService(IServiceProvider services)
        {
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }

        private Process CreateStream(string url)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{url}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
        
        public async Task SendAsync(string path)
        {
            // Create FFmpeg using the previous example
            Ffmpeg = CreateStream(path);
            await using var output = Ffmpeg.StandardOutput.BaseStream;
            await using var discord = Client.CreatePCMStream(AudioApplication.Mixed);
            try { await output.CopyToAsync(discord); }
            finally { await discord.FlushAsync(); }
        }
        
    }
}