namespace MultiTerminal.Connections.API.Spot
{
    public class QuoteSpot
    {
        public string e { get; set; }    // Тип події (EventType)
        public long E { get; set; }      // Час події (EventTime)
        public string s { get; set; }    // Символ (Symbol)
        public string p { get; set; }    // Зміна ціни (PriceChange)
        public string P { get; set; }    // Зміна відсотків ціни (PriceChangePercent)
        public string w { get; set; }    // Зважена середня ціна (WeightedAveragePrice)
        public string x { get; set; }    // Попередній закритий (PreviousClose)
        public string c { get; set; }    // Поточний закритий (CurrentClose)
        public string Q { get; set; }    // Обсяг закриття (CloseQuantity)
        public string b { get; set; }    // Найкраща ціна купівлі (BestBidPrice)
        public string B { get; set; }    // Обсяг найкращої ціни купівлі (BestBidQuantity)
        public string a { get; set; }    // Найкраща ціна продажу (BestAskPrice)
        public string A { get; set; }    // Обсяг найкращої ціни продажу (BestAskQuantity)
        public string o { get; set; }    // Відкриття (Open)
        public string h { get; set; }    // Максимум (High)
        public string l { get; set; }    // Мінімум (Low)
        public string v { get; set; }    // Обсяг (Volume)
        public string q { get; set; }    // Обсяг угод (QuoteVolume)
        public long O { get; set; }      // Час відкриття (OpenTime)
        public long C { get; set; }      // Час закриття (CloseTime)
        public long F { get; set; }      // Перший ідентифікатор угоди (FirstTradeId)
        public long L { get; set; }      // Останній ідентифікатор угоди (LastTradeId)
        public long n { get; set; }      // Кількість угод (TradeCount)

        public override string ToString()
        {
            return $"Тип події: {e}\n" +
               $"\nЧас події: {E}\n" +
               $"\nСимвол: {s}\n" +
               $"\nЗміна ціни: {p}\n" +
               $"\nЗміна відсотків ціни: {P}\n" +
               $"\nЗважена середня ціна: {w}\n" +
               $"\nПопередній закритий: {x}\n" +
               $"\nПоточний закритий: {c}\n" +
               $"\nОбсяг закриття: {Q}\n" +
               $"\nНайкраща ціна купівлі: {b}\n" +
               $"\nОбсяг найкращої ціни купівлі: {B}\n" +
               $"\nНайкраща ціна продажу: {a}\n" +
               $"\nОбсяг найкращої ціни продажу: {A}\n" +
               $"\nВідкриття: {o}\n" +
               $"\nМаксимум: {h}\n" +
               $"\nМінімум: {l}\n" +
               $"\nОбсяг: {v}\n" +
               $"\nОбсяг угод: {q}\n" +
               $"\nЧас відкриття: {O}\n" +
               $"\nЧас закриття: {C}\n" +
               $"\nПерший ідентифікатор угоди: {F}\n" +
               $"\nОстанній ідентифікатор угоди: {L}\n" +
               $"\nКількість угод: {n}\n\n\n";
        }
    }
}
