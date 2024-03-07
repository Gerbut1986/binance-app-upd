namespace MultiTerminal.Connections
{
    internal class OptChain
    {
        public ulong Id { get; set; }
        public string Type { get; set; }
        public string DateExpiration { get; set; }
        public string VolumeCont_Calls { get; set; }
        public string VolumeUSDT_Calls { get; set; }
        public string OpenICont_Calls { get; set; } //OI in contract
        public string OpenIUSDT_Calls { get; set; }
        public string BidSize_Calls { get; set; }
        public string AskSize_Calls { get; set; }
        public string Bid_Calls { get; set; }
        public string Ask_Calls { get; set; }
        public string Strike { get; set; }                // STRIKE
        public string Bid_Puts { get; set; }
        public string Ask_Puts { get; set; }
        public string AskSize_Puts { get; set; }
        public string BidSize_Puts { get; set; }
        public string OpenIUSDT_Puts { get; set; }
        public string OpenICont_Puts { get; set; }
        public string VolumeUSDT_Puts { get; set; }
        public string VolumeCont_Puts { get; set; }
    }
}
