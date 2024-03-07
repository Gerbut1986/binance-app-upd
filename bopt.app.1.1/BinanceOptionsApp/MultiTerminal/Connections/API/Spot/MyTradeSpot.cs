namespace MultiTerminal.Connections.API.Spot
{
    public class MyTradeSpot
    {
        public string Symbol { get; set; }
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int OrderListId { get; set; } // Unless OCO, the value will always be -1
        public string Price { get; set; }
        public string Qty { get; set; }
        public string QuoteQty { get; set; }
        public string Commission { get; set; }
        public string CommissionAsset { get; set; }
        public long Time { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsMaker { get; set; }
        public bool IsBestMatch { get; set; }
    }
}
