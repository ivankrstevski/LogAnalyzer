using LogAnalyzer.Classes;
using LogAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LogAnalyzer
{
    class Program
    {
        private static readonly NameValueCollection AppSettings = ConfigurationManager.AppSettings;

        static void Main(string[] args)
        {
            var fileName = $"{AppSettings["logFilesBaseFolder"]}/{AppSettings["currentFileName"]}";
            var resultList = new List<LogItem>();
            var tasks = new List<Task>();

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    ConsoleLogger.LogError();
                    return;
                }

                var clientAddresses = LogFileProcessor.GetGroupedIpAddresses(fileName);

                if (clientAddresses.Count == 0)
                {
                    ConsoleLogger.LogError();
                    return;
                }

                var groupedAddresses = clientAddresses.GroupBy(x => x);

                foreach (var address in groupedAddresses)
                {
                    tasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            var result = Dns.GetHostEntryAsync(address.Key);
                            resultList.Add(new LogItem(result.Result.HostName, address.Key, address.Count()));
                        }
                        catch (Exception)
                        {
                            resultList.Add(new LogItem(null, address.Key, address.Count()));
                        }
                    }));
                }

                Task task = Task.WhenAll(tasks);

                ConsoleLogger.LogMessage(AppSettings["pleaseWait"]);

                task.Wait();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    resultList = resultList.OrderByDescending(x => x.NumberOfHits).ToList();

                    foreach (var resultListItem in resultList)
                    {
                        ConsoleLogger.LogMessage(resultListItem.ToString());
                    }

                    ConsoleLogger.LogMessage(AppSettings["dnsSuccess"]);
                }
                else
                {
                    ConsoleLogger.LogError();
                }
            }
            catch (Exception)
            {
                ConsoleLogger.LogError();
            }

            Console.ReadLine();
        }
    }
}
