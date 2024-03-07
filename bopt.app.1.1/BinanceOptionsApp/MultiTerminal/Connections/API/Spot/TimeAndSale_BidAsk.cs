namespace MultiTerminal.Connections.API.Spot
{
    public class TimeAndSale_BidAsk
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public ulong EventTime { get; set; }//
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

        public decimal AskBuyMarket { get; set; }
        public decimal AskBuyLimit { get; set; }
        public decimal BoomBuyLimit { get; set; }
        public decimal BoomSellLimit { get; set; }
        public decimal BoomLimit { get; set; }
        public decimal BoomMarket { get; set; }

        public decimal BidSellMarket { get; set; }
        public decimal BidSellLimit { get; set; }

        public long CustomUpdateID { get; set; }  // copy from BookTicker 
    }
}
