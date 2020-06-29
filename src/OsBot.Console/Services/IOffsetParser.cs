using System;

namespace OsBot.Console.Services
{
    public interface IOffsetParser
    {
        DateTime Parse(string input, DateTime offsetFrom);
    }
}