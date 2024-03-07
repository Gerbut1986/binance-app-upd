namespace MultiTerminal.Connections.API.Spot
{
    internal class TimeAndSale
    {
        public ulong Id { get; set; }
        public string e { get; set; } // Тип події (trade)
        public ulong E { get; set; }   // Час події (в мілісекундах від початку епохи Unix)
        public string s { get; set; } // Символ торгів (наприклад, BTCUSDT)
        public long t { get; set; }   // Унікальний ідентифікатор угоди
        public string p { get; set; } // Ціна угоди
        public string q { get; set; } // Обсяг угоди
        public long b { get; set; }   // Унікальний ідентифікатор покупця
        public long a { get; set; }   // Унікальний ідентифікатор продавця
        public long T { get; set; }   // Час угоди (в мілісекундах від початку епохи Unix)
        public bool m { get; set; }   // Якщо true, то покупець є творцем лімітного ордера
        public bool M { get; set; }   // Якщо true, то продавець є творцем лімітного ордера

        public override string ToString()
        {
            return $"Id: {Id} | " +
                   $"Event Type: {e} | " +
                   $"Event Time: {E} | " +
                   $"Trading Symbol: {s} | " +
                   $"Trade ID: {t} | " +
                   $"Trade Price: {p} | " +
                   $"Trade Quantity: {q} | " +
                   $"Buyer ID: {b} | " +
                   $"Seller ID: {a} | " +
                   $"Trade Time: {T} | " +
                   $"Buyer is Market Maker: {m} | " +
                   $"Seller is Market Maker: {M} |";
        }
    }
}
