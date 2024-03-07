using Newtonsoft.Json;
using System.Collections.Generic;

namespace MultiTerminal.Connections.Details.Binance
{
    #region Testnet models:
    public class BalanceTestnet
    {
        [JsonProperty("accountAlias")]
        public string AccountAlias { get; set; }
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
        [JsonProperty("crossWalletBalance")]
        public decimal CrossWalletBalance { get; set; }
        [JsonProperty("crossUnPnl")]
        public decimal CrossUnPnl { get; set; }
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        [JsonProperty("maxWithdrawAmount")]
        public decimal MaxWithdrawAmount { get; set; }
        [JsonProperty("marginAvailable")]
        public bool MarginAvailable { get; set; }
        [JsonProperty("updateTime")]
        public long UpdateTime { get; set; }
    }

    #endregion

    #region Account[spot]
    public class Balance
    {
        public string Asset { get; set; }
        public string Free { get; set; }
        public string Locked { get; set; }
    }

    public class CommissionRates
    {
        public string Maker { get; set; }
        public string Taker { get; set; }
        public string Buyer { get; set; }
        public string Seller { get; set; }
    }

    public class AccountInfo
    {
        public int MakerCommission { get; set; }
        public int TakerCommission { get; set; }
        public int BuyerCommission { get; set; }
        public int SellerCommission { get; set; }
        public CommissionRates CommissionRates { get; set; }
        public bool CanTrade { get; set; }
        public bool CanWithdraw { get; set; }
        public bool CanDeposit { get; set; }
        public bool Brokered { get; set; }
        public bool RequireSelfTradePrevention { get; set; }
        public bool PreventSor { get; set; }
        public long UpdateTime { get; set; }
        public string AccountType { get; set; }
        public List<Balance> Balances { get; set; }
        public List<string> Permissions { get; set; }
        public long Uid { get; set; }
    }
    #endregion
    public class AccountInfo_Old
    {
        [JsonProperty("makerCommission")]
        public int MakerCommission { get; set; }
        [JsonProperty("takerCommission")]
        public int TakerCommission { get; set; }
        [JsonProperty("buyerCommission")]
        public int BuyerCommission { get; set; }
        [JsonProperty("sellerCommission")]
        public int SellerCommission { get; set; }
        [JsonProperty("canTrade")]
        public bool CanTrade { get; set; }
        [JsonProperty("canWithdraw")]
        public bool CanWithdraw { get; set; }
        [JsonProperty("canDeposit")]
        public bool CanDeposit { get; set; }
        [JsonProperty("balances")]
        public IEnumerable<BinanceBalance> Balances { get; set; }
    }
    public class BinanceBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }
        [JsonProperty("free")]
        public decimal Free { get; set; }
        [JsonProperty("locked")]
        public decimal Locked { get; set; }
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class RateLimit
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int limit { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceFilter
    {
        public string filterType { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string tickSize { get; set; }
        public string minQty { get; set; }
        public string maxQty { get; set; }
        public string stepSize { get; set; }
        public string minNotional { get; set; }
        public int? limit { get; set; }
        public int? maxNumAlgoOrders { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceSymbol
    {
        public string symbol { get; set; }
        public string status { get; set; }
        public string baseAsset { get; set; }
        public int baseAssetPrecision { get; set; }
        public string quoteAsset { get; set; }
        public int quotePrecision { get; set; }
        public List<string> orderTypes { get; set; }
        public bool icebergAllowed { get; set; }
        public List<BinanceFilter> filters { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class ExchangeInfo
    {
        public string timezone { get; set; }
        public long serverTime { get; set; }
        public List<RateLimit> rateLimits { get; set; }
        public List<object> exchangeFilters { get; set; }
        public List<BinanceSymbol> symbols { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceFill
    {
        public string price { get; set; }
        public string qty { get; set; }
        public string commission { get; set; }
        public string commissionAsset { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceNewOrder
    {
        [JsonProperty("symbol", Required = Required.Default)]
        public string symbol { get; set; }
        [JsonProperty("orderId", Required = Required.Default)]
        public long orderId { get; set; }
        [JsonProperty("clientOrderId", Required = Required.Default)]
        public string clientOrderId { get; set; }
        [JsonProperty("transactTime", Required = Required.Default)]
        public long transactTime { get; set; }
        [JsonProperty("price", Required = Required.Default)]
        public string price { get; set; }
        [JsonProperty("origQty", Required = Required.Default)]
        public string origQty { get; set; }
        [JsonProperty("executedQty", Required = Required.Default)]
        public string executedQty { get; set; }
        [JsonProperty("cummulativeQuoteQty", Required = Required.Default)]
        public string cummulativeQuoteQty { get; set; }
        [JsonProperty("status", Required = Required.Default)]
        public string status { get; set; }
        [JsonProperty("timeInForce", Required = Required.Default)]
        public string timeInForce { get; set; }
        [JsonProperty("type", Required = Required.Default)]
        public string type { get; set; }
        [JsonProperty("side", Required = Required.Default)]
        public string side { get; set; }
        [JsonProperty("fills", Required = Required.Default)]
        public List<BinanceFill> fills { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceTestnetNewOrder
    {
        [JsonProperty("clientOrderId", Required = Required.Default)]
        public string clientOrderId { get; set; }
        [JsonProperty("cumQty", Required = Required.Default)]
        public string cumQty { get; set; }
        [JsonProperty("cumQuote", Required = Required.Default)]
        public string cumQuote { get; set; }
        [JsonProperty("executedQty", Required = Required.Default)]
        public string executedQty { get; set; }
        [JsonProperty("orderId", Required = Required.Default)]
        public long orderId { get; set; }
        [JsonProperty("avgPrice", Required = Required.Default)]
        public string avgPrice { get; set; }
        [JsonProperty("origQty", Required = Required.Default)]
        public string origQty { get; set; }
        [JsonProperty("price", Required = Required.Default)]
        public string price { get; set; }
        [JsonProperty("reduceOnly", Required = Required.Default)]
        public bool reduceOnly { get; set; }
        [JsonProperty("side", Required = Required.Default)]
        public string side { get; set; }
        [JsonProperty("positionSide", Required = Required.Default)]
        public string positionSide { get; set; }
        [JsonProperty("status", Required = Required.Default)]
        public string status { get; set; }
        [JsonProperty("stopPrice", Required = Required.Default)]
        public string stopPrice { get; set; }
        [JsonProperty("closePosition", Required = Required.Default)]
        public bool closePosition { get; set; }
        [JsonProperty("symbol", Required = Required.Default)]
        public string symbol { get; set; }
        [JsonProperty("timeInForce", Required = Required.Default)]
        public string timeInForce { get; set; }
        [JsonProperty("type", Required = Required.Default)]
        public string type { get; set; }
        [JsonProperty("origtype", Required = Required.Default)]
        public string origtype { get; set; }
        [JsonProperty("activatePrice", Required = Required.Default)]
        public string activatePrice { get; set; }
        [JsonProperty("priceRate", Required = Required.Default)]
        public string priceRate { get; set; }
        [JsonProperty("updateTime", Required = Required.Default)]
        public long updateTime { get; set; }
        [JsonProperty("workingType", Required = Required.Default)]
        public string workingType { get; set; }
        [JsonProperty("priceProtect", Required = Required.Default)]
        public bool priceProtect { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceServerTime
    {
        public long serverTime { get; set; }

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceListenKey
    {
        public string listenKey { get; set; }

    }

    public class BinanceListenKeyOption
    {
        public string listenKey { get; set; }
        public long expiration { get; set; }

    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceUserAsset
    {
        public string asset { get; set; }
        public decimal borrowed { get; set; }//Показує кількість цього активу, яку користувач позичив у рамках маржевого торгівельного обліку.
        public decimal free { get; set; }//Показує кількість цього активу, яка є доступною для торгівлі чи виведення. 
        public decimal interest { get; set; }// це відсоткові виплати за використання позичених коштів. позиченою кількістю цього активу. 
        public decimal locked { get; set; } //Вказує кількість цього активу, яка заблокована або зафіксована і не може бути використана для торгівлі чи виведення. Зазвичай, це заблокована кількість активу в рамках відкритих угод чи інших обмежень.
        public decimal netAsset { get; set; }//Представляє чистий актив для цього конкретного активу, розрахований як різниця між доступною кількістю (Free) та заблокованою кількістю (Locked). Може бути використано для визначення загального чистого активу користувача після урахування всіх активів.

    }

     [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceMarginAccountDetails
    {
        public bool tradeEnabled { get; set; }
        public bool transferEnabled { get; set; }
        public bool borrowEnabled { get; set; }
        public string marginLevel { get; set; }
        public string totalAssetOfBtc { get; set; }
        public string totalLiabilityOfBtc { get; set; }
        public string totalNetAssetOfBtc { get; set; }
        public List<BinanceUserAsset> userAssets { get; set; }
        public string collateralMarginLevel { get; set; }
        public string totalCollateralValueInUSDT { get; set; }
        public string accountType { get; set; }
        // Old attr
        //    public bool borrowEnabled { get; set; }
        //    public decimal marginLevel { get; set; }
        //    public decimal totalAssetOfBtc { get; set; }
        //    public decimal totalLiabilityOfBtc { get; set; }
        //    public decimal totalNetAssetOfBtc { get; set; }
        //    public bool tradeEnabled { get; set; }
        //    public bool transferEnabled { get; set; }
        //    public List<BinanceUserAsset> userAssets { get; set; }
        //}
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class BinanceMarginNewOrder
    {
        [JsonProperty("symbol", Required = Required.Default)]
        public string Symbol { get; set; }
        [JsonProperty("orderId", Required = Required.Default)]
        public long OrderId { get; set; }
        [JsonProperty("clientOrderId", Required = Required.Default)]
        public string ClientOrderId { get; set; }
        [JsonProperty("transactTime", Required = Required.Default)]
        public long TransactTime { get; set; }
        [JsonProperty("price", Required = Required.Default)]
        public string Price { get; set; }
        [JsonProperty("origQty", Required = Required.Default)]
        public string OrigQty { get; set; }
        [JsonProperty("executedQty", Required = Required.Default)]
        public string ExecutedQty { get; set; }
        [JsonProperty("cummulativeQuoteQty", Required = Required.Default)]
        public string CummulativeQuoteQty { get; set; }
        [JsonProperty("status", Required = Required.Default)]
        public string Status { get; set; }
        [JsonProperty("timeInForce", Required = Required.Default)]
        public string TimeInForce { get; set; }
        [JsonProperty("type", Required = Required.Default)]
        public string Type { get; set; }
        [JsonProperty("side", Required = Required.Default)]
        public string Side { get; set; }
        [JsonProperty("marginBuyBorrowAmount", Required = Required.Default)]
        public int MarginBuyBorrowAmount { get; set; }
        [JsonProperty("marginBuyBorrowAsset", Required = Required.Default)]
        public string MarginBuyBorrowAsset { get; set; }
        [JsonProperty("isIsolated", Required = Required.Default)]
        public bool IsIsolated { get; set; }
        [JsonProperty("fills", Required = Required.Default)]
        public List<BinanceFill> Fills { get; set; }
    }

    #region Futures:
    public class AccountInfoFuture
    {
        [JsonProperty("feeTier")]
        public int FeeTier { get; set; }
        [JsonProperty("canTrade")]
        public bool CanTrade { get; set; }
        [JsonProperty("canDeposit")]
        public bool CanDeposit { get; set; }
        [JsonProperty("canWithdraw")]
        public bool CanWithdraw { get; set; }
        [JsonProperty("updateTime")]
        public long UpdateTime { get; set; }
        [JsonProperty("multiAssetsMargin")]
        public bool MultiAssetsMargin { get; set; }
        [JsonProperty("tradeGroupId")]
        public int TradeGroupId { get; set; }
        [JsonProperty("totalInitialMargin")]
        public string TotalInitialMargin { get; set; }
        [JsonProperty("totalMaintMargin")]
        public string TotalMaintMargin { get; set; }
        [JsonProperty("totalWalletBalance")]
        public string TotalWalletBalance { get; set; }
        [JsonProperty("totalUnrealizedProfit")]
        public string TotalUnrealizedProfit { get; set; }
        [JsonProperty("totalMarginBalance")]
        public string TotalMarginBalance { get; set; }
        [JsonProperty("totalPositionInitialMargin")]
        public string TotalPositionInitialMargin { get; set; }
        [JsonProperty("totalOpenOrderInitialMargin")]
        public string TotalOpenOrderInitialMargin { get; set; }
        [JsonProperty("totalCrossWalletBalance")]
        public string TotalCrossWalletBalance { get; set; }
        [JsonProperty("totalCrossUnPnl")]
        public string TotalCrossUnPnl { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
        [JsonProperty("maxWithdrawAmount")]
        public string MaxWithdrawAmount { get; set; }
        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }
        [JsonProperty("positions")]
        public List<PositionFuture> Positions { get; set; }
    }
    public class Asset
    {
        [JsonProperty("asset")]
        public string asset { get; set; }
        [JsonProperty("walletBalance")]
        public string WalletBalance { get; set; }
        [JsonProperty("unrealizedProfit")]
        public string UnrealizedProfit { get; set; }
        [JsonProperty("marginBalance")]
        public string MarginBalance { get; set; }
        [JsonProperty("maintMargin")]
        public string MaintMargin { get; set; }
        [JsonProperty("initialMargin")]
        public string InitialMargin { get; set; }
        [JsonProperty("positionInitialMargin")]
        public string PositionInitialMargin { get; set; }
        [JsonProperty("openOrderInitialMargin")]
        public string OpenOrderInitialMargin { get; set; }
        [JsonProperty("crossWalletBalance")]
        public string CrossWalletBalance { get; set; }
        [JsonProperty("crossUnPnl")]
        public string CcrossUnPnl { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
        [JsonProperty("maxWithdrawAmount")]
        public string MaxWithdrawAmount { get; set; }
        [JsonProperty("marginAvailable")]
        public bool MarginAvailable { get; set; }
        [JsonProperty("updateTime")]
        public long UpdateTime { get; set; }
    }

    public class PositionFuture
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("initialMargin")]
        public string InitialMargin { get; set; }
        [JsonProperty("maintMargin")]
        public string MaintMargin { get; set; }
        [JsonProperty("unrealizedProfit")]
        public string UnrealizedProfit { get; set; }
        [JsonProperty("positionInitialMargin")]
        public string PositionInitialMargin { get; set; }
        [JsonProperty("openOrderInitialMargin")]
        public string OpenOrderInitialMargin { get; set; }
        [JsonProperty("leverage")]
        public string Leverage { get; set; }
        [JsonProperty("isolated")]
        public bool Isolated { get; set; }
        [JsonProperty("entryPrice")]
        public string EntryPrice { get; set; }
        [JsonProperty("maxNotional")]
        public string MaxNotional { get; set; }
        [JsonProperty("bidNotional")]
        public string BidNotional { get; set; }
        [JsonProperty("askNotional")]
        public string AskNotional { get; set; }
        [JsonProperty("positionSide")]
        public string PositionSide { get; set; }
        [JsonProperty("positionAmt")]
        public string PositionAmt { get; set; }
        [JsonProperty("updateTime")]
        public long UpdateTime { get; set; }
    }

}

public class BinanceNewOrderF
{
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("cumQty")]
    public string CumQty { get; set; }

    [JsonProperty("cumQuote")]
    public string CumQuote { get; set; }

    [JsonProperty("executedQty")]
    public string ExecutedQty { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("avgPrice")]
    public string AvgPrice { get; set; }

    [JsonProperty("origQty")]
    public string OrigQty { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    [JsonProperty("side")]
    public string Side { get; set; }

    [JsonProperty("positionSide")]
    public string PositionSide { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("stopPrice")]
    public decimal StopPrice { get; set; }

    [JsonProperty("closePosition")]
    public bool ClosePosition { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("timeInForce")]
    public string TimeInForce { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("origType")]
    public string OrigType { get; set; }

    [JsonProperty("activatePrice")]
    public decimal ActivatePrice { get; set; }

    [JsonProperty("priceRate")]
    public decimal PriceRate { get; set; }

    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }

    [JsonProperty("workingType")]
    public string WorkingType { get; set; }

    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; }

    [JsonProperty("selfTradePreventionMode")]
    public string SelfTradePreventionMode { get; set; }

    [JsonProperty("goodTillDate")]
    public long GoodTillDate { get; set; }
}
#endregion 

#region Future [COIN-M]^
public class AccountInfo_Coin_M_Future
{
    [JsonProperty("assets")]
    public List<Asset_CoinM> Assets { get; set; }
    [JsonProperty("positions")]
    public List<PositionCoin_M> Positions { get; set; }
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }
}

public class Asset_CoinM
{
    [JsonProperty("asset")]
    public string AssetName { get; set; }
    [JsonProperty("walletBalance")]
    public decimal WalletBalance { get; set; }//*
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }//*
    [JsonProperty("marginBalance")]
    public decimal MarginBalance { get; set; }
    [JsonProperty("maintMargin")]
    public decimal MaintMargin { get; set; }
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }//*
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaxWithdrawAmount { get; set; }
    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }
    [JsonProperty("crossUnPnl")]
    public decimal CrossUnPnl { get; set; }
    [JsonProperty("availableBalance")]
    public decimal AvailableBalance { get; set; }
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }
}

public class PositionCoin_M
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }
    [JsonProperty("positionAmt")]
    public decimal PositionAmt { get; set; }//*
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }
    [JsonProperty("maintMargin")]
    public decimal MaintMargin { get; set; }
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }
    [JsonProperty("leverage")]
    public int Leverage { get; set; }
    [JsonProperty("isolated")]
    public bool Isolated { get; set; }
    [JsonProperty("positionSide")]
    public string PositionSide { get; set; }
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }
    [JsonProperty("notionalValue")] 
    public decimal NotionalValue { get; set; }
    [JsonProperty("breakEvenPrice")] 
    public decimal BreakEvenPrice { get; set; }
    [JsonProperty("maxQty")]
    public decimal MaxQty { get; set; }
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public class COIN_M_NewOrder
{
    public string clientOrderId { get; set; }
    public string cumQty { get; set; }
    public string cumBase { get; set; }
    public string executedQty { get; set; }
    public long orderId { get; set; }
    public decimal avgPrice { get; set; }
    public string origQty { get; set; }
    public decimal price { get; set; }
    public bool reduceOnly { get; set; }
    public string side { get; set; }
    public string positionSide { get; set; }
    public string status { get; set; }
    public decimal stopPrice { get; set; }
    public bool closePosition { get; set; }
    public string symbol { get; set; }
    public string pair { get; set; }
    public string timeInForce { get; set; }
    public string type { get; set; }
    public string origType { get; set; }
    public decimal activatePrice { get; set; }
    public decimal priceRate { get; set; }
    public long updateTime { get; set; }
    public string workingType { get; set; }
    public bool priceProtect { get; set; }
}

// Modify Order Futures:
public class BatchOrderFuture
{
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("cumQty")]
    public string CumQty { get; set; }

    [JsonProperty("cumQuote")]
    public string CumQuote { get; set; }

    [JsonProperty("executedQty")]
    public string ExecutedQty { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("avgPrice")]
    public string AvgPrice { get; set; }

    [JsonProperty("origQty")]
    public string OrigQty { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    [JsonProperty("side")]
    public string Side { get; set; }

    [JsonProperty("positionSide")]
    public string PositionSide { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("stopPrice")]
    public string StopPrice { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("timeInForce")]
    public string TimeInForce { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("origType")]
    public string OrigType { get; set; }

    [JsonProperty("activatePrice")]
    public string ActivatePrice { get; set; }

    [JsonProperty("priceRate")]
    public string PriceRate { get; set; }

    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }

    [JsonProperty("workingType")]
    public string WorkingType { get; set; }

    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; }

    [JsonProperty("selfTradePreventionMode")]
    public string SelfTradePreventionMode { get; set; }

    [JsonProperty("goodTillDate")]
    public long GoodTillDate { get; set; }
}

public class ErrorMessage
{
    [JsonProperty("code")]
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string Msg { get; set; }
}
#endregion
#region Options:
public class AccountInfoOption
{
    public List<AssetDetail> asset { get; set; }
    // public List<GreekDetail> greek { get; set; }
    public string riskLevel { get; set; }
    public long time { get; set; }
}

public class AssetDetail
{
    public string asset { get; set; }
    public string marginBalance { get; set; }
    public string equity { get; set; }
    public string available { get; set; }
    public string locked { get; set; }
    public string unrealizedPNL { get; set; }
}

public class GreekDetail
{
    public string Underlying { get; set; }
    public string Delta { get; set; }
    public string Gamma { get; set; }
    public string Theta { get; set; }
    public string Vega { get; set; }
}
#endregion

#region Margin Account: 
public class MarginOpenOrders
{
    public string symbol { get; set; }
    public bool isIsolated { get; set; }
    public string origClientOrderId { get; set; }
    public int orderId { get; set; }
    public long orderListId { get; set; }
    public string clientOrderId { get; set; }
    public string price { get; set; }
    public string origQty { get; set; }
    public string executedQty { get; set; }
    public string cummulativeQuoteQty { get; set; }
    public string status { get; set; }
    public string timeInForce { get; set; }
    public string type { get; set; }
    public string side { get; set; }
    public string selfTradePreventionMode { get; set; }
    public string contingencyType { get; set; }
    public string listStatusType { get; set; }
    public string listOrderStatus { get; set; }
    public string listClientOrderId { get; set; }
    public long transactionTime { get; set; }
    public List<MarginOpenOrders> orders { get; set; }
    public List<OrderReport> orderReports { get; set; }
}

public class OrderReport
{
    public string symbol { get; set; }
    public string origClientOrderId { get; set; }
    public int orderId { get; set; }
    public long orderListId { get; set; }
    public string clientOrderId { get; set; }
    public string price { get; set; }
    public string origQty { get; set; }
    public string executedQty { get; set; }
    public string cummulativeQuoteQty { get; set; }
    public string status { get; set; }
    public string timeInForce { get; set; }
    public string type { get; set; }
    public string side { get; set; }
    public string stopPrice { get; set; }
    public string icebergQty { get; set; }
}

#region DELETE /sapi/v1/margin/openOrders
public class DeleteOrdersResult
{
    public string symbol { get; set; }
    public bool isIsolated { get; set; }
    public string origClientOrderId { get; set; }
    public int orderId { get; set; }
    public long orderListId { get; set; }
    public string clientOrderId { get; set; }
    public string price { get; set; }
    public string origQty { get; set; }
    public string executedQty { get; set; }
    public string cummulativeQuoteQty { get; set; }
    public string status { get; set; }
    public string timeInForce { get; set; }
    public string type { get; set; }
    public string side { get; set; }
    public string selfTradePreventionMode { get; set; }
    public string contingencyType { get; set; }
    public string listStatusType { get; set; }
    public string listOrderStatus { get; set; }
    public string listClientOrderId { get; set; }
    public long transactionTime { get; set; }
    public List<DeleteOrdersResult> orders { get; set; }
    public List<OrderReportDel> orderReports { get; set; }
}

public class OrderReportDel
{
    public string symbol { get; set; }
    public string origClientOrderId { get; set; }
    public int orderId { get; set; }
    public long orderListId { get; set; }
    public string clientOrderId { get; set; }
    public string price { get; set; }
    public string origQty { get; set; }
    public string executedQty { get; set; }
    public string cummulativeQuoteQty { get; set; }
    public string status { get; set; }
    public string timeInForce { get; set; }
    public string type { get; set; }
    public string side { get; set; }
    public string stopPrice { get; set; }
    public string icebergQty { get; set; }
}
#endregion
#endregion