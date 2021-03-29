using LogAnalyzer.Models;
using System;

namespace LogAnalyzer.Classes
{
    public static class ConsoleLogger
    {
        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogResultMessage(LogItem logItem, string noHostFound)
        {
            Console.WriteLine($"{logItem.HostName ?? noHostFound} ({logItem.IpAddress}) - {logItem.NumberOfHits}");
        }
    }
}
