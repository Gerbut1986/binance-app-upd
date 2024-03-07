namespace MultiTerminal.Connections.API.Spot
{
    public class QuoteAskBid
    {
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public decimal AskVol { get; set; }
        public decimal BidVol { get; set; }
        public string EventTime { get; set; }
    }
}
