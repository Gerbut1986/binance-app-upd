namespace MultiTerminal.Connections.Details.Binance
{
    public class BinanceEndPoint
    {
        public string Value;
        public string Method;
        public bool isSigned;

        public BinanceEndPoint(string Value, string Method, bool isSigned)
        {
            this.Value = Value;
            this.Method = Method;
            this.isSigned = isSigned;
        }
    }
    public static class EndPoints
    {
        #region Testnet Spot:
        public static readonly BinanceEndPoint SpotCreateListenKeyTest = new BinanceEndPoint("/sapi/v1/userDataStream", "POST", false);
        #endregion

        #region Testnet Binancefuture:
        public static readonly BinanceEndPoint BalanceT = new BinanceEndPoint("/fapi/v2/balance", "GET", true);
        public static readonly BinanceEndPoint AccountInformationT = new BinanceEndPoint("/fapi/v2/account", "GET", true);
        public static readonly string TestConnectivityT = "/fapi/v1/ping";
        public static readonly BinanceEndPoint CheckServerTimeT = new BinanceEndPoint("/fapi/v1/time", "GET", false);
        public static readonly BinanceEndPoint ExchangeInformationT = new BinanceEndPoint("/fapi/v1/exchangeInfo", "GET", false);
        public static readonly BinanceEndPoint SpotCreateListenKeyT = new BinanceEndPoint("/fapi/v1/listenKey", "POST", false);
        public static readonly BinanceEndPoint NewOrderT = new BinanceEndPoint("/fapi/v1/order", "POST", true);
        public static readonly BinanceEndPoint NewOrderTestT = new BinanceEndPoint("/fapi/v1/order/test", "POST", true);
        //public static readonly string NewOrderTestT = "/fapi/v1/order/test";
        public static readonly string QueryOrderT = "/fapi/v1/order";
        public static readonly string CancelOrderT = "/fapi/v1/order";
        public static readonly BinanceEndPoint CurrentOpenOrderT = new BinanceEndPoint("/fapi/v1/openOrder", "GET", true);
        public static readonly string CurrentOpenOrdersT = "/fapi/v1/openOrders";
        public static readonly string AllOrdersT = "/fapi/v1/allOrders";
        public static readonly string AllOpenOrdersT = "/fapi/v1/allOpenOrders";
        #endregion

        #region General Endpoints
        public static readonly string TestConnectivity = "/api/v1/ping";
        public static readonly BinanceEndPoint CheckServerTime = new BinanceEndPoint("/api/v1/time", "GET", false);
        public static readonly BinanceEndPoint ExchangeInformation = new BinanceEndPoint("/api/v1/exchangeInfo", "GET", false);
        #endregion

        #region Market Data Endpoints
        public static readonly string OrderBook = "/api/v1/depth";
        public static readonly string AggregateTrades = "/api/v1/aggTrades";
        public static readonly string Candlesticks = "/api/v1/klines";
        public static readonly string TickerPriceChange24H = "/api/v1/ticker/24hr";
        public static readonly string AllPrices = "/api/v1/ticker/allPrices";
        public static readonly string OrderBookTicker = "/api/v1/ticker/allBookTickers";
        public static readonly string TradingRules = "https://gist.githubusercontent.com/Ninj0r/3029b9d635f8f81f5ffab9cc9df5cc61/raw/810530a2118e5d8cdcfcc4d220349976a0acf131/tradingRules_20171022.json";
        #endregion

        #region Account Endpoints
        public static readonly BinanceEndPoint NewOrder = new BinanceEndPoint("/api/v3/order", "POST", true);
        public static readonly string NewOrderTest = "/api/v3/order/test";
        public static readonly string QueryOrder = "/api/v3/order";
        public static readonly string CancelOrder = "/api/v3/order";
        public static readonly string CurrentOpenOrders = "/api/v3/openOrders";
        public static readonly string AllOrders = "/api/v3/allOrders";
        public static readonly BinanceEndPoint AccountInformation = new BinanceEndPoint("/api/v3/account", "GET", true);
        public static readonly BinanceEndPoint MarginAccountInformation = new BinanceEndPoint("/sapi/v1/account", "GET", true);
        public static readonly string TradeList = "/api/v3/myTrades";

        public static readonly string Withdraw = "/wapi/v1/withdraw.html";
        public static readonly string DepositHistory = "/wapi/v1/getDepositHistory.html";
        public static readonly string WithdrawHistory = "/wapi/v1/getWithdrawHistory.html";
        #endregion

        #region User Stream Endpoints

        public static readonly BinanceEndPoint SpotCreateListenKey = new BinanceEndPoint("/api/v3/userDataStream", "POST", false);
        public static readonly BinanceEndPoint SpotUpdateListenKey = new BinanceEndPoint("/api/v3/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint SpotDeleteListenKey = new BinanceEndPoint("/api/v3/userDataStream", "DELETE", false);
        public static readonly BinanceEndPoint MarginCreateListenKey = new BinanceEndPoint("/sapi/v1/userDataStream", "POST", false);
        public static readonly BinanceEndPoint MarginUpdateListenKey = new BinanceEndPoint("/sapi/v1/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint MarginDeleteListenKey = new BinanceEndPoint("/sapi/v1/userDataStream", "DELETE", false);
        //https://api.binance.com/sapi/v1/userDataStream  
        #endregion

        #region Margin Account/Trade

        public static readonly BinanceEndPoint QueryCrossMarginAccountDetails = new BinanceEndPoint("/sapi/v1/margin/account", "GET", true);
        public static readonly BinanceEndPoint MarginAccountNewOrder = new BinanceEndPoint("/sapi/v1/margin/order", "POST", true);
        public static readonly BinanceEndPoint MarginAccountOrderModify = new BinanceEndPoint("/sapi/v1/margin/order", "PUT", true);
        public static readonly BinanceEndPoint MarginAccountOrderDelete = new BinanceEndPoint("/sapi/v1/margin/order", "DELETE", true);
        public static readonly BinanceEndPoint MarginAccountOpenOrders = new BinanceEndPoint("/sapi/v1/margin/openOrders", "GET", true);
        public static readonly BinanceEndPoint MarginAccountDeleteOpenOrders = new BinanceEndPoint("/sapi/v1/margin/openOrders", "DELETE", true);
        #endregion

        #region Binance Options [Test method]:
        public static readonly BinanceEndPoint OptionsTrade = new BinanceEndPoint("/eapi/v1/trades", "GET", false);

        public static readonly BinanceEndPoint AccountInformationOptions = new BinanceEndPoint("/eapi/v1/account", "GET", true);
        #endregion

        #region User Stream Endpoints [SPOT/MARGIN] OPTIONS

        public static readonly BinanceEndPoint SpotCreateListenKeyOption = new BinanceEndPoint("/eapi/v1/listenKey", "POST", false);
        public static readonly BinanceEndPoint SpotUpdateListenKeyOption = new BinanceEndPoint("/eapi/v3/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint SpotDeleteListenKeyOption = new BinanceEndPoint("/eapi/v3/userDataStream", "DELETE", false);
        public static readonly BinanceEndPoint MarginCreateListenKeyOption = new BinanceEndPoint("/eapi/v1/userDataStream", "POST", false);
        public static readonly BinanceEndPoint MarginUpdateListenKeyOption = new BinanceEndPoint("/eapi/v1/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint MarginDeleteListenKeyOption = new BinanceEndPoint("/eapi/v1/userDataStream", "DELETE", false);
        // /eapi/v1/openInterest get
        #endregion

        #region Account Endpoints [SPOT/MARGIN] OPTIONS
        public static readonly BinanceEndPoint NewOrderOption = new BinanceEndPoint("/api/v3/order", "POST", true);
        public static readonly string NewOrderTestOption = "/api/v3/order/test";
        public static readonly string QueryOrderOption = "/api/v3/order";
        public static readonly string CancelOrderOption = "/api/v3/order";
        public static readonly string CurrentOpenOrdersOption = "/api/v3/openOrders";
        public static readonly string AllOrdersOption = "/api/v3/allOrders";
        public static readonly BinanceEndPoint AccountInformationOption = new BinanceEndPoint("/eapi/v1/marginAccount", "GET", true);
        public static readonly BinanceEndPoint MarginAccountInformationOption = new BinanceEndPoint("/eapi/v1/account", "GET", true);
        public static readonly string TradeListOption = "/api/v3/myTrades";

        public static readonly string WithdrawOption = "/wapi/v1/withdraw.html";
        public static readonly string DepositHistoryOption = "/wapi/v1/getDepositHistory.html";
        public static readonly string WithdrawHistoryOption = "/wapi/v1/getWithdrawHistory.html";
        #endregion

        #region Account Endpoints [USD-M / COIN-M] FUTURES
        public static readonly BinanceEndPoint CheckServerTimeFuture = new BinanceEndPoint("/fapi/v1/time", "GET", false);
        public static readonly BinanceEndPoint BatchOrderFuture = new BinanceEndPoint("/fapi/v1/batchOrders", "POST", true);
        public static readonly string NewOrderTestFuture = "/api/v3/order/test";
        public static readonly string QueryOrderFuture = "/api/v3/order";
        public static readonly string CancelOrderFuture = "/api/v3/order";
        public static readonly string CurrentOpenOrdersFuture = "/api/v3/openOrders";
        public static readonly string AllOrdersFuture = "/api/v3/allOrders";
        public static readonly BinanceEndPoint AccountInformationFuture = new BinanceEndPoint("/fapi/v2/account", "GET", true);
        public static readonly BinanceEndPoint COIN_MAccountInformationF = new BinanceEndPoint("/dapi/v1/account", "GET", true);
        public static readonly string TradeListFuture = "/api/v3/myTrades";


        public static readonly BinanceEndPoint NewOrderFuture = new BinanceEndPoint("/fapi/v1/order", "POST", true);
        public static readonly BinanceEndPoint DeleteOrderFuture = new BinanceEndPoint("/fapi/v1/order", "DELETE", true);
        public static readonly BinanceEndPoint Coin_M_NewOrder = new BinanceEndPoint("/dapi/v1/order", "POST", true);
        public static readonly BinanceEndPoint OrderModifyFutureUsd_M = new BinanceEndPoint("/fapi/v1/order", "PUT", true);

        public static readonly string WithdrawFuture = "/wapi/v1/withdraw.html";
        public static readonly string DepositHistoryFuture = "/wapi/v1/getDepositHistory.html";
        public static readonly string WithdrawHistoryFuture = "/wapi/v1/getWithdrawHistory.html";
        #endregion

        #region User Stream Endpoints Futures

        public static readonly BinanceEndPoint SpotCreateListenKeyFuture = new BinanceEndPoint("/fapi/v1/listenKey", "POST", false);
        public static readonly BinanceEndPoint SpotUpdateListenKeyFuture = new BinanceEndPoint("/fapi/v3/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint SpotDeleteListenKeyFuture = new BinanceEndPoint("/fapi/v3/userDataStream", "DELETE", false);
        public static readonly BinanceEndPoint MarginCreateListenKeyFuture = new BinanceEndPoint("/sapi/v1/userDataStream", "POST", false);
        public static readonly BinanceEndPoint MarginUpdateListenKeyFuture = new BinanceEndPoint("/sapi/v1/userDataStream", "PUT", false);
        public static readonly BinanceEndPoint MarginDeleteListenKeyFuture = new BinanceEndPoint("/sapi/v1/userDataStream", "DELETE", false);


        #endregion
    }
}
