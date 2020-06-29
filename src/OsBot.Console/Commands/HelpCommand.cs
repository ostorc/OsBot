using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using OsBot.Core.Command;

namespace OsBot.Console.Commands
{
    internal class HelpCommand : IChatCommand
    {
        public const string CommandText = "help";

        private readonly CommandCollection _commands;

        public HelpCommand(CommandCollection commands)
        {
            _commands = commands;
        }

        public async Task Execute(DiscordClient client, DiscordMessage message, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Available commands:");
            foreach (var command in _commands.AvailableCommands)
            {
                sb.AppendLine($"\t{command}");
            }

            await client.SendMessageAsync(message.Channel, sb.ToString());
        }
    }
}