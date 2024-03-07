using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceOptionsApp.MultiTerminal.Connections.API.Future
{
    public class BookTickerData
    {
        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("u")]
        public long UpdateId { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("b")]
        public string BidPrice { get; set; }

        [JsonProperty("B")]
        public string BidQuantity { get; set; }

        [JsonProperty("a")]
        public string AskPrice { get; set; }

        [JsonProperty("A")]
        public string AskQuantity { get; set; }

        [JsonProperty("T")]
        public long TransactionTime { get; set; }

        [JsonProperty("E")]
        public long EventTime { get; set; }

        // Custom attr:
        public long SynchronizationTime { get; set; }
        public long TradeTime { get; set; }
    }

    public class BookTickerF
    {
        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("data")]
        public BookTickerData Data { get; set; }
    }
}
