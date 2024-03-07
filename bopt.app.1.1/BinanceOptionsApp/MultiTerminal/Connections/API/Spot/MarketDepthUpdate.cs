namespace MultiTerminal.Connections.API.Spot
{
    using System.Collections.Generic;

    public class MarketDepthUpdate
    {
        public long lastUpdateId { get; set; }
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }
    }
}
