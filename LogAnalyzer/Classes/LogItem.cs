namespace LogAnalyzer.Models
{
    public class LogItem
    {
        public LogItem(string HostName, string IpAddress, int NumberOfHits)
        {
            this.HostName = HostName;
            this.IpAddress = IpAddress;
            this.NumberOfHits = NumberOfHits;
        }

        public string HostName { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfHits { get; set; }
    }
}
