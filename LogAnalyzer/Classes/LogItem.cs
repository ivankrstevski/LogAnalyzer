using System.Collections.Specialized;
using System.Configuration;

namespace LogAnalyzer.Models
{
    public class LogItem
    {
        private readonly NameValueCollection AppSettings;

        public LogItem(string HostName, string IpAddress, int NumberOfHits)
        {
            this.HostName = HostName;
            this.IpAddress = IpAddress;
            this.NumberOfHits = NumberOfHits;
            this.AppSettings = ConfigurationManager.AppSettings;
        }

        public string HostName { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfHits { get; set; }

        public override string ToString()
        {
            return $"{HostName ?? AppSettings["noSuchHost"]} ({IpAddress}) - {NumberOfHits}";
        }
    }
}
