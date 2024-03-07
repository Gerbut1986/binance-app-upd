namespace MultiTerminal.Connections.API.Spot
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class DepthSpot
    {
        public string subscribe { get; set; }
        public DepthData data { get; set; }
    }

    public class DepthData
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("u")]
        public long FirstUpdateId { get; set; }
        [JsonProperty("U")]
        public long FinalUpdateId { get; set; }
        [JsonProperty("b")]
        public List<List<string>> Bids { get; set; }
        [JsonProperty("a")]
        public List<List<string>> Asks { get; set; }
    }
}
