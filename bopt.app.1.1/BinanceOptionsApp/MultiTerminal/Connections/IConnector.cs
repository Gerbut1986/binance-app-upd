using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MultiTerminal.Connections
{
    public class TickEventArgs : EventArgs
    {
        public string Symbol { get; set; }
        public string SymbolId { get; set; }
        public string SubscriptionId { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public List<List<string>> Bids { get; set; }
        public List<List<string>> Asks { get; set; }
    }
    public class TickEventArgsOptions : EventArgs
    {
        public string Symbol { get; set; }
        public string SymbolId { get; set; }
        public string SubscriptionId { get; set; }
        public string DateExpiration { get; set; }
        public string VolumeCont_Calls { get; set; }
        public string VolumeUSDT_Calls { get; set; }
        public string OpenICont_Calls { get; set; }    //OI in contract
        public string OpenIUSDT_Calls { get; set; }
        public string BidSize_Calls { get; set; }
        public string AskSize_Calls { get; set; }
        public string Bid_Calls { get; set; }
        public string Ask_Calls { get; set; }
        public string Strike { get; set; }                // STRIKE
        public string Bid_Puts { get; set; }
        public string Ask_Puts { get; set; }
        public string AskSize_Puts { get; set; }
        public string BidSize_Puts { get; set; }
        public string OpenIUSDT_Puts { get; set; }
        public string OpenICont_Puts { get; set; }
        public string VolumeUSDT_Puts { get; set; }
        public string VolumeCont_Puts { get; set; }
    }

    public class TickEventArgsFutures : EventArgs
    {
        public ConcurrentDictionary<decimal, API.Future.MarketDepthUpdateFuture> MarketDepthList { get; set; }
        public string Symbol { get; set; }
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
        public List<List<string>> Bids { get; set; }
        public List<List<string>> Asks { get; set; }
    }

    public enum OrderSide
    {
        Buy,
        Sell
    }
    public enum OrderType
    {
        Market = 0,
        Limit = 1,
        Stop = 2
    }
    public enum FillPolicy
    {
        FOK = 0,
        IOK = 1,
        FILL = 2,
        GTC = 3
    }
    public class OrderInformation
    {
        public string Id { get; set; }
        public OrderSide Side { get; set; }
        public string Symbol { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal Volume { get; set; }
        public decimal PnL { get; set; }
        public int Magic { get; set; }
        public int Track { get; set; }
        public DateTime OpenTime { get; set; }
        public decimal StopLoss { get; set; }
        public decimal TakeProfit { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class OrderOpenResult
    {
        public string Id { get; set; }
        public decimal OpenPrice { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string Error { get; set; }
    }
    public class OrderCloseResult
    {
        public decimal ClosePrice { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string Error { get; set; }
    }
    public class OrderModifyResult
    {
        public string Lot { get; set; }
        public string Side { get; set; }
        public decimal OpenPrice { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public string Error { get; set; }
    }
    public interface IConnectorLogger
    {
        void LogInfo(string msg);
        void LogError(string msg);
        void LogWarning(string msg);
        void LogOrderSuccess(string msg);
    }
    internal interface IConnector
    {
        event EventHandler LoggedIn;
        event EventHandler<TickEventArgs> Tick;
        event EventHandler<TickEventArgsOptions> TickOption;
        event EventHandler<TickEventArgsFutures> TickFuture;
        event EventHandler LoggedOut;

        void Start();
        void Stop(bool wait);
        bool IsLoggedIn { get; }
        DateTime CurrentTime { get; }
        string ViewId { get; }
        FillPolicy Fill { get; set; }

        decimal? Balance { get; }
        decimal? Equity { get; }

        void Subscribe(string symbol, string id, string algo = "");
        void Unsubscribe(string symbol, string id);

        List<OrderInformation> GetOrders(string symbol, int magic, int track);
        OrderOpenResult Open(string symbol, decimal price, decimal lot, FillPolicy policy, OrderSide side, int magic, int slippage, int track, OrderType type, int lifetimeMs);
        OrderModifyResult Modify(string symbol, string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot);
        OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs);
        bool OrderDelete(string symbol, string orderId, string origClientOrderId);
    }

    internal class OrdersStatistic
    {
        public List<OrderInformation> Orders { get; set; }
        public decimal Profit { get; set; }
        public decimal Volume { get; set; }
        public int OrdersCount { get; set; }
        public int BuysCount { get; set; }
        public int SellsCount { get; set; }
        public OrderInformation OrderBuy { get; set; }
        public OrderInformation OrderSell { get; set; }
        public decimal BuyProfit { get; set; }
        public decimal SellProfit { get; set; }
        public OrdersStatistic(List<OrderInformation> orders, decimal bid, decimal ask)
        {
            Orders = orders;
            OrdersCount = orders.Count;
            foreach (var order in orders)
            {
                if (order.Side == OrderSide.Buy)
                {
                    BuyProfit += bid - order.OpenPrice;
                    Profit += bid - order.OpenPrice;
                    Volume += order.Volume;
                    OrderBuy = order;
                    BuysCount++;
                }
                else
                {
                    SellProfit += order.OpenPrice - ask;
                    Profit += order.OpenPrice - ask;
                    Volume -= order.Volume;
                    OrderSell = order;
                    SellsCount++;
                }
            }
            if (OrdersCount > 1) Profit /= orders.Count;
        }
    }

#pragma warning disable CS0067
    internal class ProxyConnector : IConnector
    {
        public bool IsLoggedIn
        {
            get
            {
                return false;
            }
        }

        public DateTime CurrentTime => DateTime.MinValue;
        public string ViewId => "Proxy";

        public FillPolicy Fill { get; set; }

        public decimal? Balance => null;
        public decimal? Equity => null;

        public event EventHandler LoggedIn; 
        public event EventHandler LoggedOut;
        public event EventHandler<TickEventArgs> Tick;
        public event EventHandler<TickEventArgsOptions> TickOption;
        public event EventHandler<TickEventArgsFutures> TickFuture;

        public OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs)
        {
            return new OrderCloseResult() { Error = "Not implemented" };
        }

        public List<OrderInformation> GetOrders(string symbol, int magic, int track)
        {
            return new List<OrderInformation>();
        }
        public OrderModifyResult Modify(string symbol, string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot)
        {
            return new OrderModifyResult() { Error = "Not implemented" };
        }
        public OrderOpenResult Open(string symbol, decimal price, decimal lot, FillPolicy policy, OrderSide side, int magic, int slippage, int track, OrderType type, int lifetimeMs)
        {
            return new OrderOpenResult() { Error = "Not implemented" };
        }

        public bool OrderDelete(string symbol, string orderId, string origClientOrderId)
        {
            return false;
        }

        public void Start()
        {
        }

        public void Stop(bool wait)
        {
        }

        public void Subscribe(string symbol, string id, string algo = "")
        {
        }

        public void Unsubscribe(string symbol, string id)
        {
        }
    }
#pragma warning restore CS0067
}
