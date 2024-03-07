namespace MultiTerminal.Connections.FutureModels
{
    using System.Collections.Generic;

    public class ExchangeInfoResponse
    {
        public List<RateLimit> rateLimits { get; set; }
        public long serverTime { get; set; }
        public List<AssetInfo> assets { get; set; }
        public List<SymbolInfo> symbols { get; set; }
        public string timezone { get; set; }

        public class RateLimit
        {
            public string interval { get; set; }
            public int intervalNum { get; set; }
            public int limit { get; set; }
            public string rateLimitType { get; set; }
        }

        public class AssetInfo
        {
            public string asset { get; set; }
            public bool marginAvailable { get; set; }
            public int? autoAssetExchange { get; set; }
        }

        public class SymbolInfo
        {
            public string Symbol { get; set; }
            public string Pair { get; set; }
            public string ContractType { get; set; }
            public long DeliveryDate { get; set; }
            public long OnboardDate { get; set; }
            public string Status { get; set; }
            public string BaseAsset { get; set; }
            public string QuoteAsset { get; set; }
            public string MarginAsset { get; set; }
            public int PricePrecision { get; set; }
            public int QuantityPrecision { get; set; }
            public int BaseAssetPrecision { get; set; }
            public int QuotePrecision { get; set; }
            public string UnderlyingType { get; set; }
            public List<string> UnderlyingSubType { get; set; }
            public int SettlePlan { get; set; }
            public string TriggerProtect { get; set; }
            public List<Filter> Filters { get; set; }
            public List<string> OrderType { get; set; }
            public List<string> TimeInForce { get; set; }
            public string LiquidationFee { get; set; }
            public string MarketTakeBound { get; set; }
        }

        public class Filter
        {
            public string FilterType { get; set; }
            public string MaxPrice { get; set; }
            public string MinPrice { get; set; }
            public string TickSize { get; set; }
            public string MaxQty { get; set; }
            public string MinQty { get; set; }
            public string StepSize { get; set; }
            public int? Limit { get; set; }
            public string Notional { get; set; }
            public string MultiplierUp { get; set; }
            public string MultiplierDown { get; set; }
            public int? MultiplierDecimal { get; set; }
        }
    }

}
