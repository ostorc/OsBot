using Microsoft.EntityFrameworkCore;

namespace OsBot.Console.DAL
{
    public class BotContext : DbContext
    {
#nullable disable
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }
#nullable restore

        public DbSet<RemindNote> RemindNotes { get; set; }
    }
}