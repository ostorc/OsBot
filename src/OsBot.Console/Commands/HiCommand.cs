using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using OsBot.Console.DAL;
using OsBot.Core;
using OsBot.Core.Command;

namespace OsBot.Console.Commands
{
    public class HiCommand : IChatCommand
    {
        public const string CommandText = "hi";

        private readonly BotContext _context;

        public HiCommand(BotContext context)
        {
            _context = context;
        }

        public async Task Execute(DiscordClient client, DiscordMessage message, CancellationToken cancellationToken)
        {
            var cnt = await _context.RemindNotes.CountAsync(cancellationToken);

            var userMention = DiscordHelper.GetMention(message.Author.Id);

            await _context.SaveChangesAsync(cancellationToken);
            await client.SendMessageAsync(message.Channel, $"Hi {userMention}! Count: {cnt}");
        }
    }
}