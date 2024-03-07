namespace MultiTerminal.Connections.API.Spot
{
    using System.Collections.Generic;

    public class Depth
    {
        // [EventType] "e" - це означає, що це оновлення Стакану цін Level 2:
        public string e { get; set; }
        // [EventTime] "E" - це час події в мілісекундах від початку епохи Unix:
        public long E { get; set; }
        // [Symbol] "s" - це символ пари торгів,
        // до якого відноситься це оновлення (в даному випадку, пара торгів BTCUSDT, тобто Bitcoin до долара США):
        public string s { get; set; }
        // [FirstUpdateId] "U" - це останній оновлений час Стакану перед цим оновленням ("U" - updateId):
        public long U { get; set; }
        // [FinalUpdateId] "u" - це оновлений час Стакану після цього оновлення ("u" - lastUpdateId):
        public long u { get; set; }
        // [Bids] "b" - список списків цін Bid:
        public List<List<string>> b { get; set; }
        // [Asks] "a" - список списків цін Ask:
        public List<List<string>> a { get; set; }
    }
}
