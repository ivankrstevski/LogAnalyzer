using LogAnalyzer.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace LogAnalyzer.Classes
{
    public static class ConsoleLogger
    {
        private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static void LogMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogError()
        {
            Console.WriteLine(appSettings["error"]);
        }
    }
}
