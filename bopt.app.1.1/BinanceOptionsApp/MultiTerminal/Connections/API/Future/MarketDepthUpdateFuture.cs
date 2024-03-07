namespace MultiTerminal.Connections.API.Future
{
    public class MarketDepthUpdateFuture
    {
        public string LastTimeUpd { get; set; }
        public decimal Volume { get; set; }
        public decimal MaxVol { get; set; }
        public decimal MinVol { get; set; }
        public int RankAsk { get; set; }
        public int RankBid { get; set; }
        public int Type { get; set; }

        public MarketDepthUpdateFuture DeepCopy()
        {
            return new MarketDepthUpdateFuture
            {
                LastTimeUpd = this.LastTimeUpd != null ? string.Copy(this.LastTimeUpd) : null,
                Volume = this.Volume,
                MaxVol = this.MaxVol,
                MinVol = this.MinVol,
                RankAsk = this.RankAsk,
                RankBid = this.RankBid,
                Type = this.Type
                // Add more properties if necessary, and if they are reference types, perform a deep copy for those as well
            };
        }
    }
}
