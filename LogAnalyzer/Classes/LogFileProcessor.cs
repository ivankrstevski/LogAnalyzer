using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace LogAnalyzer.Classes
{
    public static class LogFileProcessor
    {
        private static readonly NameValueCollection AppSettings = ConfigurationManager.AppSettings;

        public static List<string> GetGroupedIpAddresses(string fileName)
        {
            var clientAddresses = new List<string>();
            var columnTaken = false;
            var columnIndex = -1;

            try
            {
                using (StreamReader reader = File.OpenText(fileName))
                {
                    var line = string.Empty;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("#Fields") && !columnTaken)
                        {
                            var splittedFieldsLine = line.Split(' ');

                            columnIndex = Array.FindIndex(splittedFieldsLine, x => x == AppSettings["columnName"]);

                            columnTaken = true;
                        }
                        else if (!line.StartsWith("#"))
                        {
                            var splittedLine = line.Split(' ');

                            clientAddresses.Add(splittedLine[columnIndex - 1]);
                        }
                    }
                }

                return clientAddresses;
            }
            catch (Exception)
            {
                return clientAddresses;
            }
        }
    }
}
