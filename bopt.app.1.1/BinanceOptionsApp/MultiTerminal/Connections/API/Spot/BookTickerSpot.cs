namespace MultiTerminal.Connections.API.Spot
{
    using Newtonsoft.Json;

    public class BookTickerSpot
    {
        [JsonProperty("u")]
        public long UpdateId { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("b")]
        public decimal BestBidPrice { get; set; }

        [JsonProperty("B")]
        public decimal BestBidQuantity { get; set; }

        [JsonProperty("a")]
        public decimal BestAskPrice { get; set; }

        [JsonProperty("A")]
        public decimal BestAskQuantity { get; set; }

        public long CustomTimeLenta { get; set; }   // copy from TimeAndSale_BidAsk
        public long SynchronizationTime { get; set; }   // time for synchronization
    }
}
