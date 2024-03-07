using MultiTerminal.Connections.API.Spot;

namespace MultiTerminal.Connections.API.Future
{
    public class TimeAndSale_BidAskFuture
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public ulong EventTime { get; set; }
        public string Symbol { get; set; }
        public long Ticket { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public long BuyerID { get; set; }
        public long SellerID { get; set; }
        public long DealTime { get; set; }
        public bool IsBuyLimit { get; set; }
        public bool IsSellLimit { get; set; }

        public string EventDate { get; set; }

        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}
