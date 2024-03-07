namespace MultiTerminal.Connections.API.Future
{
    public class QuoteFuture
    {
        public string Stream { get; set; }
        public QuoteDetails Data { get; set; }
    }

    public class QuoteDetails
    {
        public string e { get; set; } // Event type
        public long E { get; set; }   // Event time
        public string s { get; set; } // Symbol
        public string p { get; set; } // Price change
        public string P { get; set; } // Price change percentage
        public string w { get; set; } // Weighted average price
        public string c { get; set; } // Last price
        public string Q { get; set; } // Last quantity
        public string o { get; set; } // Opening price
        public string h { get; set; } // Highest price
        public string l { get; set; } // Lowest price
        public string v { get; set; } // Total traded base asset volume
        public string q { get; set; } // Total traded quote asset volume
        public long O { get; set; }   // Statistics open time
        public long C { get; set; }   // Statistics close time
        public int F { get; set; }    // First trade ID
        public int L { get; set; }    // Last trade ID
        public int n { get; set; }    // Total number of trades
    }
}
