namespace MultiTerminal.Connections
{
    public class OpenInterestModel
    {
        public string stream { get; set; }
        public OpenInterestDetails[] data { get; set; }
    }

    public class OpenInterestDetails
    {
        public string e { get; set; }   // Event type
        public long E { get; set; }     // Event time
        public string s { get; set; }   // Option symbol
        public string o { get; set; }   // Open interest in contracts
        public string h { get; set; }   // Open interest in USDT
    }
}
