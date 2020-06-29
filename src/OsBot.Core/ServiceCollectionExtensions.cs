using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OsBot.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordOptions(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.Configure<DiscordConfig>(configuration.GetSection(DiscordConfig.ConfigName));
            return serviceCollection;
        }

        public static IServiceCollection AddDiscordBot(this IServiceCollection serviceCollection,
            Action<CommandBuilder> configure)
        {
            serviceCollection.AddHostedService<DiscordWorker>();
            serviceCollection.AddSingleton<IDiscordBot, DiscordBot>();
            serviceCollection.AddSingleton<DiscordWrapper>();

            var builder = new CommandBuilder(serviceCollection);
            configure(builder);

            serviceCollection.AddSingleton(builder.ToCollection());

            return serviceCollection;
        }
    }
}