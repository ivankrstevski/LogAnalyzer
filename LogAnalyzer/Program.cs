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
        private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        static void Main(string[] args)
        {
            var fileName = $"{appSettings["logFilesBaseFolder"]}/{appSettings["currentFileName"]}";
            var resultList = new List<LogItem>();
            var tasks = new List<Task>();

            try
            {
                var clientAddresses = LogFileProcessor.GetGroupedIpAddresses(fileName);

                if (clientAddresses.Count == 0)
                {
                    ConsoleLogger.LogMessage(appSettings["error"]);
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

                ConsoleLogger.LogMessage(appSettings["pleaseWait"]);

                task.Wait();

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    resultList = resultList.OrderByDescending(x => x.NumberOfHits).ToList();

                    foreach (var resultListItem in resultList)
                    {
                        ConsoleLogger.LogResultMessage(resultListItem, appSettings["noSuchHost"]);
                    }

                    ConsoleLogger.LogMessage(appSettings["dnsSuccess"]);
                }
                else
                {
                    ConsoleLogger.LogMessage(appSettings["error"]);
                }
            }
            catch (Exception)
            {
                ConsoleLogger.LogMessage(appSettings["error"]);
            }

            Console.ReadLine();
        }
    }
}
