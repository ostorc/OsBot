using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OsBot.Console.DAL;
using OsBot.Core;

namespace OsBot.Console.Workers
{
    public class RemindWorker : BackgroundService
    {
        private readonly BotContext _context;
        private readonly ILogger<RemindWorker> _logger;
        private readonly DiscordClient _client;

        public RemindWorker(BotContext context, DiscordWrapper discordWrapper, ILogger<RemindWorker> logger)
        {
            _context = context;
            _logger = logger;
            _client = discordWrapper.Client;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var cnt = await _context.RemindNotes
                    .CountAsync(r => r.Status == RemindStatus.New, stoppingToken);
                _logger.LogInformation("Count of NEW remind notes: {0}", cnt);

                var date = DateTime.Now;
                var remindToBeSent = await _context.RemindNotes
                    .Where(r => r.Status == RemindStatus.New)
                    .Where(r => r.When <= date)
                    .ToListAsync(stoppingToken);

                if (remindToBeSent.Any())
                {
                    foreach (var remindNote in remindToBeSent)
                    {
                        var channel = await _client.GetChannelAsync(remindNote.ChannelId);
                        if (channel == null)
                        {
                            _logger.LogError("Couldn't find channel with id: {0}", remindNote.ChannelId);
                            remindNote.Status = RemindStatus.Failed;
                            continue;
                        }

                        var mention = DiscordHelper.GetMention(remindNote.AuthorId);
                        var link = DiscordHelper.GetMessageLink(channel.GuildId, channel.Id, remindNote.MessageId);

                        await _client.SendMessageAsync(channel,
                            $"{mention}: Remind you of {link}"
                        );

                        remindNote.Status = RemindStatus.Send;
                    }

                    _context.RemindNotes.UpdateRange(remindToBeSent);
                    await _context.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(15000, stoppingToken);
            }
        }
    }
}