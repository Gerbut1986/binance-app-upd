using Newtonsoft.Json;

namespace MultiTerminal.Connections.API.Future
{
    public class TimeAndSaleFuture
    {
        public string Stream { get; set; }
        public TradeDetails Data { get; set; }
    }

    public class TradeDetails
    {
        public string e { get; set; }   // Event type
        public long E { get; set; }     // Event time
        public long T { get; set; }     // Trade time
        public string s { get; set; }   // Symbol
        public long t { get; set; }     // Trade ID
        public string p { get; set; }   // Price
        public string q { get; set; }   // Quantity
        public string X { get; set; }   // Trade execution type
        public bool m { get; set; }     // Buyer is the maker
    }
   
}
