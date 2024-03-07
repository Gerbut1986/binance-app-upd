namespace MultiTerminal.Connections.API.Future
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class PartialDepthFuture
    {
        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("data")]
        public DepthUpdateEventData Data { get; set; }
    }

    public class DepthUpdateEventData
    {
        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventTime { get; set; }

        [JsonProperty("T")]
        public long TransactionTime { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("U")]
        public long FirstUpdateId { get; set; }

        [JsonProperty("u")]
        public long FinalUpdateId { get; set; }

        [JsonProperty("pu")]
        public long PreviousLastUpdateId { get; set; }

        [JsonProperty("b")]
        public List<List<string>> Bids { get; set; }

        [JsonProperty("a")]
        public List<List<string>> Asks { get; set; }
    }
}
