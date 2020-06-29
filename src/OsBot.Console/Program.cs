using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using OsBot.Console.Commands;
using OsBot.Console.DAL;
using OsBot.Console.Services;
using OsBot.Console.Services.Implementation;
using OsBot.Console.Workers;
using OsBot.Core;

namespace OsBot.Console
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(c =>
                {
                    c.ClearProviders();
                    c.AddConsole().AddDebug();
                })
                .ConfigureAppConfiguration((builder, config) => { })
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureConfig(hostContext.Configuration, services);
                    ConfigureServices(services);
                });
        }

        private static void ConfigureConfig(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDiscordOptions(configuration);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BotContext>(opts =>
                opts.UseSqlite("Data Source=bot.db"));

            services.AddDiscordBot(c =>
            {
                c.AddCommand<HiCommand>(HiCommand.CommandText);
                c.AddCommand<HelpCommand>(HelpCommand.CommandText);
                c.AddCommand<RemindCommand>(RemindCommand.CommandText);
            });

            services.AddTransient<IOffsetParser, OffsetParser>();

            services.AddHostedService<RemindWorker>();
        }
    }
}