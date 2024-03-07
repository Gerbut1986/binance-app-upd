using MultiTerminal.Connections.API.Future;

namespace MultiTerminal.Connections.API.Spot
{
    public class MarketDepthUpdateSpot
    {
        public long LastUpdateId { get; set; } = 0;
        public string LastTimeUpd { get; set; } = "0";
        public decimal Volume { get; set; } = 0;
        public decimal MaxVol { get; set; } = 0;
        public decimal MinVol { get; set; } = 0;
        public int RankAsk { get; set; } = 0;
        public int RankBid { get; set; } = 0;
        public int Type { get; set; } = 0;

        public MarketDepthUpdateSpot DeepCopy()
        {
            return new MarketDepthUpdateSpot
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
