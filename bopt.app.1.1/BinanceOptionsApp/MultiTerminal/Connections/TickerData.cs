namespace MultiTerminal.Connections
{
    public class TickerData
    {
        public int Id { get; set; }
        public string Stream { get; set; }
        public TickerDetails Data { get; set; }
    }

    public class TickerDetails
    {
        public string e { get; set; }   // Event type
        public long E { get; set; }     // Event time
        public long T { get; set; }     // Transaction time
        public string s { get; set; }   // Option symbol
        public string o { get; set; }   // 24-hour opening price
        public string h { get; set; }   // Highest price
        public string l { get; set; }   // Lowest price
        public string c { get; set; }   // Latest price
        public string V { get; set; }   // Trading volume (in contracts) (Volume (Cont))
        public string A { get; set; }   // Trade amount (in quote asset) (Volume (USDT))
        public string P { get; set; }   // Price change percent
        public string p { get; set; }   // Price change
        public string Q { get; set; }   // Volume of last completed trade (in contracts)
        public string F { get; set; }   // First trade ID
        public string L { get; set; }   // Last trade ID
        public int n { get; set; }      // Number of trades
        public string bo { get; set; }  // The best buy price  (Bid)!
        public string ao { get; set; }  // The best sell price (Ask)!
        public string bq { get; set; }  // The best buy quantity
        public string aq { get; set; }  // The best sell quantity
        public string b { get; set; }   // Buy Implied volatility
        public string a { get; set; }   // Sell Implied volatility
        public string d { get; set; }   // Delta
        public string t { get; set; }   // Theta
        public string g { get; set; }   // Gamma
        public string v { get; set; }   // Vega
        public string vo { get; set; }  // Implied volatility
        public string mp { get; set; }  // Mark price
        public string hl { get; set; }  // Buy Maximum price
        public string ll { get; set; }  // Sell Minimum price
        public string eep { get; set; } // Estimated strike price (return estimated strike price half an hour before exercise)
    }
}
