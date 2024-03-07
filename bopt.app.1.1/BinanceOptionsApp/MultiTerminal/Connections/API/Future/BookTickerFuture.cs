namespace MultiTerminal.Connections.API.Future
{
    public class BookTickerFuture
    {
        public string e { get; set; }   // event type
        public long u { get; set; }     // order book updateId
        public long E { get; set; }     // event time
        public long T { get; set; }     // transaction time
        public string s { get; set; }   // symbol
        public string b { get; set; }   // best bid price
        public string B { get; set; }   // best bid qty
        public string a { get; set; }   // best ask price
        public string A { get; set; }   // best ask qty
    }
}
