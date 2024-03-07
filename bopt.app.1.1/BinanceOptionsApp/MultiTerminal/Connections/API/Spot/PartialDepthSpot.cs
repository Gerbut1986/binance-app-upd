namespace MultiTerminal.Connections.API.Spot
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class PartialDepthSpot
    {
        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("data")]
        public DepthUpdateEventData Data { get; set; }
    }

    public class DepthUpdateEventData
    {
        [JsonProperty("bids")]
        public List<List<string>> Bids { get; set; }

        [JsonProperty("asks")]
        public List<List<string>> Asks { get; set; }
    }
}

