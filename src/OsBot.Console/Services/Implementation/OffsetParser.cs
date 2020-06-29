using System;
using System.Linq;

namespace OsBot.Console.Services.Implementation
{
    internal sealed class OffsetParser : IOffsetParser
    {
        private static readonly (string Text, int TimePeriod)[] RecognizableText = new[]
        {
            ("min", 1),
            ("mins", 1),
            ("minute", 1),
            ("minutes", 1),
            ("h", 60),
            ("hour", 60),
            ("hours", 60),
            ("day", 60 * 24),
            ("days", 60 * 24),
            ("month", 1),
            ("months", 1)
        };

        private DateTime ParseOffset(string modifierText, string timeFrame, DateTime offsetFrom)
        {
            if (!int.TryParse(modifierText, out var modifier))
                throw new ArgumentOutOfRangeException(nameof(modifierText));

            var recognizedTime = RecognizableText
                .FirstOrDefault(t => t.Text == timeFrame);

            if (recognizedTime == default)
                throw new ArgumentOutOfRangeException(nameof(recognizedTime), "Couldn't recognize time frame");

            var actualOffset = modifier * recognizedTime.TimePeriod;

            if (recognizedTime.Text.StartsWith("month")) // Not really nice hack
                return offsetFrom.AddMonths(actualOffset);

            return offsetFrom.AddDays(actualOffset);
        }

        public DateTime Parse(string input, DateTime offsetFrom)
        {
            // TODO: Verify correctness
            var split = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (split.Length < 2 || (split[0] == "in" && split.Length < 3))
                throw new ArgumentException("Invalid input", nameof(input));

            if (split[0] == "in")
                return ParseOffset(split[1], split[2], offsetFrom);

            if (split[0] != "on")
                throw new ArgumentOutOfRangeException(nameof(input));

            return DateTime.Parse(split[1]);
        }
    }
}