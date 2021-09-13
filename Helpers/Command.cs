using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordBot.Helpers
{
    public class Command
    {
        private SocketSlashCommand _command;

        public Command(SocketSlashCommand command)
        {
            _command = command;
        }

        public SocketUser GetUser()
        {
            return (_command.User);
        }
        
        public IGuild GetGuild()
        {
            return (_command.User as IGuildUser)?.Guild;
        }

        public IChannel GetChannel()
        {
            return (_command.Channel);
        }

        public IVoiceChannel GetVoiceChannel()
        {
            return (_command.User as IGuildUser)?.VoiceChannel;
        }
        
        public Task RespondAsync( string text = null, 
            Embed[] embeds = null, 
            bool isTTS = false, 
            bool ephemeral = false, 
            AllowedMentions allowedMentions = null, 
            RequestOptions options = null, 
            MessageComponent component = null, 
            Embed embed = null)
        {
            _command.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, options, component, embed);
            return Task.CompletedTask;
        }
        
        public Task<RestFollowupMessage> FollowupAsync(
            string text = null, 
            Embed[] embeds = null, 
            bool isTTS = false, 
            bool ephemeral = false, 
                AllowedMentions allowedMentions = null, 
            RequestOptions options = null, 
                MessageComponent component = null, 
            Embed embed = null)
        {
            return _command.FollowupAsync(text, embeds, isTTS, ephemeral, allowedMentions, options, component, embed);
        }
    }
}