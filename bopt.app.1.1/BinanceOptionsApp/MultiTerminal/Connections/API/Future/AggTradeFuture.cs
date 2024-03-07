namespace MultiTerminal.Connections.API.Future
{
    using Newtonsoft.Json;

    public class AggTradeFuture
    {
        public string stream { get; set; }
        public AggregatedTradeDataFuture data { get; set; }

       // public AggregatedTradeDataFuture dataRaw { get; set; }
    }

    public class AggregatedTradeDataFuture
    {
        public int Id { get; set; }
        [JsonProperty("e")]
        public string EventType { get; set; }    // Event type
        [JsonProperty("E")]
        public ulong EventTime { get; set; }      // Event time
        [JsonProperty("s")]
        public string Symbol { get; set; }        // Symbol
        [JsonProperty("a")]
        public ulong AggTradeId { get; set; }     // Aggregate trade ID
        [JsonProperty("p")]
        public decimal Price { get; set; }        // Price
        [JsonProperty("q")]
        public decimal Volume { get; set; }       // Quantity
        [JsonProperty("f")]
        public ulong FirstTradeId { get; set; }   // First trade ID
        [JsonProperty("l")]
        public ulong LastTradeId { get; set; }    // Last trade ID
        [JsonProperty("T")]
        public ulong TradeTime { get; set; }      // Trade time
        [JsonProperty("m")]
        public bool IsMarketMaker { get; set; }   //  Was the buyer the maker?

        public decimal Ask { get; set; }
        public decimal Bid { get; set; }

        public decimal MarketBuy { get; set; }
        public decimal MarketSell { get; set; }
        public decimal BoomAskBuyerTrue { get; set; }
        public decimal BoomAskBuyerFalse { get; set; }

        public decimal BidVolBuyerTrue { get; set; }
        public decimal BidVolBuyerFalse { get; set; }
        public decimal BoomBidBuyerTrue { get; set; }
        public decimal BoomBidBuyerFalse { get; set; }

        public string EventDate { get; set; }
    }
}
