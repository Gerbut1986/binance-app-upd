namespace MultiTerminal.Connections
{
    using System;
    using System.Linq;
    using WebSocket4Net;
    using System.Threading;
    using System.Collections.Generic;
    using MultiTerminal.Connections.Models;

    public abstract class BaseCryptoClient : IConnector
    {
        private readonly List<OrderInformation> Positions = new List<OrderInformation>();

        internal IConnectorLogger logger;
        internal ManualResetEvent cancelToken;
        internal string Key;
        internal string Secret;
        internal AccountTradeType AccountTradeType;

        protected BaseCryptoClient(IConnectorLogger logger, ManualResetEvent cancelToken, CryptoConnectionModel model)
        {
            this.logger = logger;
            this.cancelToken = cancelToken;
            Key = model.Key;
            Secret = model.Secret;
            AccountTradeType = model.AccountTradeType;
            ViewId = model.Name;
        }

        internal bool _IsLoggedIn = false;
        public bool IsLoggedIn => _IsLoggedIn;
        public DateTime CurrentTime => DateTime.UtcNow;
        public string ViewId { get; }
        public FillPolicy Fill { get; set; }
        public decimal? Balance => GetBalance();
        public decimal? Equity => null;

        public event EventHandler LoggedIn;
        public event EventHandler<TickEventArgs> Tick;
        public event EventHandler<TickEventArgsOptions> TickOption;
        public event EventHandler LoggedOut;
        public event EventHandler<TickEventArgsFutures> TickFuture;

        internal abstract decimal GetBalance();
        public virtual OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs)
        {
            OrderSide closeSide = side == OrderSide.Buy ? OrderSide.Sell : OrderSide.Buy;
            var openResult = Open(symbol, price, volume, FillPolicy.FOK, closeSide, 0, slippage, 0, type, lifetimeMs);

            return new OrderCloseResult()
            {
                ClosePrice = openResult.OpenPrice,
                ExecutionTime = openResult.ExecutionTime,
                Error = openResult.Error
            };
        }
        public List<OrderInformation> GetOrders(string symbol, int magic, int track)
        {
            lock (Positions)
            {
                return Positions.Where(p => p.Symbol == symbol).ToList();
            }
        }
        internal void AddPosition(OrderInformation orderInformation)
        {
            lock (Positions)
            {
                Positions.Add(orderInformation);
            }
        }
        internal void RemovePosition(OrderInformation orderInformation)
        {
            lock (Positions)
            {
                Positions.Remove(orderInformation);
            }
        }
        internal OrderInformation GetPosition(string Symbol)
        {
            lock (Positions)
            {
                return Positions.FirstOrDefault(x => x.Symbol == Symbol);
            }
        }
        public abstract OrderModifyResult Modify(string symbol, string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot);
        public abstract OrderOpenResult Open(string symbol, decimal price, decimal lot, FillPolicy policy, OrderSide side, int magic, int slippage, int track, OrderType type, int lifetimeMs);
        public abstract bool OrderDelete(string symbol, string orderId, string origClientOrderId);
        public abstract void Subscribe(string symbol, string id, string algo = "");
        public abstract void Unsubscribe(string symbol, string id);
        public abstract void Start();
        public abstract void Stop(bool wait);

        internal void OnLoggedIn()
        {
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }
        internal void OnTick(TickEventArgs e)
        {
            Tick?.Invoke(this, e);
        }
        internal void OnTickFuture(TickEventArgsFutures e)
        {
            TickFuture?.Invoke(this, e);
        }
        internal void OnTickOptions(TickEventArgsOptions e)
        {
            TickOption?.Invoke(this, e);
        }
        internal void OnLoggedOut()
        {
            LoggedOut?.Invoke(this, EventArgs.Empty);
        }
    }

    internal class WebSocketData
    {
        public WebSocket webSocket = null;
        public bool Active = false;
        public TickEventArgs quote = null;
    }

    public class WebSocketDataOptions
    {
        public WebSocket webSocket = null;
        public bool Active = false;
        public TickEventArgsOptions quote = null;
    }
}
