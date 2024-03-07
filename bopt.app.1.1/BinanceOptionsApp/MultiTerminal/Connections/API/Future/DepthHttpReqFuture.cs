namespace MultiTerminal.Connections.API.Future
{
    using System.Collections.Generic;

    internal class DepthHttpReqFuture
    {
        public long lastUpdateId { get; set; }
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }
    }
}
