using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using OsBot.Console.DAL;
using OsBot.Console.Services;
using OsBot.Core.Command;

namespace OsBot.Console.Commands
{
    public class RemindCommand : IChatCommand
    {
        public const string CommandText = "remind";

        private readonly ILogger<RemindCommand> _logger;
        private readonly BotContext _botContext;
        private readonly IOffsetParser _parser;

        public RemindCommand(ILogger<RemindCommand> logger, BotContext botContext, IOffsetParser parser)
        {
            _logger = logger;
            _botContext = botContext;
            _parser = parser;
        }

        public async Task Execute(DiscordClient client, DiscordMessage message, CancellationToken cancellationToken)
        {
            var commandText = $"!{CommandText}";
            var content = message.Content;
            content = content.Remove(0, commandText.Length);

            DateTime when;
            try
            {
                when = _parser.Parse(content, DateTime.Now);
            }
            catch (Exception e)
            {
                _logger.LogDebug($"Couldn't parse input '{content}'", e);
                await client.SendMessageAsync(message.Channel, "Couldn't recognize when do you want to remind :(");
                return;
            }

            var remind = new RemindNote(
                message.ChannelId,
                message.Author.Id,
                message.Id,
                when
            );
            _botContext.Add(remind);
            await _botContext.SaveChangesAsync(cancellationToken);
        }
    }
}