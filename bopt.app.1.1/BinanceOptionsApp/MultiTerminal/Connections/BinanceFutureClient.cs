namespace MultiTerminal.Connections
{
    using MultiTerminal.Connections.Details.Binance;
    using MultiTerminal.Connections.API.Future;
    using MultiTerminal.Connections.Models;
    using System.Collections.Concurrent;
    using System.Security.Cryptography;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Globalization;
    using System.Diagnostics;
    using System.Windows.Threading;
    using System.Net.NetworkInformation;
    using Newtonsoft.Json.Linq;
    using global::Models.Algo;
    using Helpers.Extensions;
    using BinanceOptionsApp;
    using BinanceOptionApp;
    using System.Threading;
    using System.Net.Http;
    using Newtonsoft.Json;
    using WebSocket4Net;
    //using System.Net.WebSockets;
    using System.Text;
    using System.Linq;
    using System.Net;
    using System;
    using MultiTerminal.Connections.API.Spot;
    using BinanceOptionsApp.MultiTerminal.Connections.API.Future;

    public class BinanceFutureClient : BaseCryptoClient
    {
        public List<BinanceNewOrderF> PlacedOrdersUsd_M { get; set; } = new List<BinanceNewOrderF>();
        public List<COIN_M_NewOrder> PlacedOrdersCoin_M { get; set; } = new List<COIN_M_NewOrder>();
        public List<Asset> FutureBalances = default;
        public AccountInfoFuture AccountInfo = null, AccInfo_UsdUsd_M = null;
        public AccountInfo_Coin_M_Future Coin_MAccInfo = null;
        DateTime lastBalanceRequestTime = DateTime.MinValue;
        private decimal lastBalance = 0;
        private int ReconectCount = 0;
        private Timer timerResetReconectCount;
        private HttpClient _httpClient;
        private readonly long recvWindow = 5000;
        private long dTime = 0; // разница между временем сервера и клиента
        private BinancePingPongManager pingPongManager;

        private string symbol;

        private readonly Dictionary<string, WebSocketData> SubscribeList = new Dictionary<string, WebSocketData>();

        private DispatcherTimer timerEvent; // TIMER ***

        #region [onTick() TIMER FOR LEVEL_2 STREAM SYNCHRONIZATION]:
        private async void OnTick(object sender, EventArgs e)
        {
            bool uvaga = false, relax = false;

            if (uvaga)
            {
                if (symbol != null)  SubscribeBookTickerStream(symbol);
                  
            }

            if (relax)
            {
                isCloseBTCustom = true;
            }
            //isCloseBTCustom = relax ? true : false;

             

            await Task.Run(() =>
            {
                lock (lockObject)
                {
                    // Оновлення TasRawF з WebSocket
                    if (TasRawF != null && TasRawF.Count != 0)
                    {
                        TasCopyF = TasRawF;//var ggh = TasCopyF.Last();//var btDict = BookTickerFutureDictionary;
                        
                        //-------------------------------------
                        //------------------------------------------------
                        ask_f = QuoteAsk; bid_f = QuoteBid;

                        var tasUpdate = TasCopyF.LastOrDefault();

                        if (tasUpdate != null)
                        {
                            if (ask_f != PrevAsk || bid_f != PrevBid || tasUpdate != PrevTAS) //  
                            {
                                decimal p = 0; decimal v = 0; decimal ask = QuoteAsk, bid = QuoteBid; bool isMMbuyer = false;


                                if (tasUpdate == PrevTAS)//e.Message 
                                {
                                    tasUpdate.data.Volume = 0.0m;
                                    tasUpdate.data.Price = 0.0m;
                                    //tasUpdate.data.EventTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                    tasUpdate.data.LastTradeId = 0;
                                    tasUpdate.data.FirstTradeId = 0;
                                    //tasUpdate.data.TradeTime = tasUpdate.data.TradeTime; // Pre time
                                    tasUpdate.data.AggTradeId = 0;

                                    long timeSynchronization = (long)tasUpdate.data.EventTime;

                                    tasUpdate.data.Ask = QuoteAsk;
                                    tasUpdate.data.Bid = QuoteBid;
                                    var dt = long.Parse(tasUpdate.data.EventTime.ToString()).GetFullTime();
                                    tasUpdate.data.EventDate = $"{dt.Day}/{dt.Month} {dt.ToString("HH:mm:ss.ffffff")}";
                                    tasUpdate.data.Id = (int)(++counter);
                                    tasUpdate.data.MarketBuy = 0;
                                    tasUpdate.data.MarketSell = 0;

                                    TasF.Add(tasUpdate);

                                }
                                else
                                {
                                    v = tasUpdate.data.Volume;
                                    p = tasUpdate.data.Price;
                                    isMMbuyer = tasUpdate.data.IsMarketMaker;

                                    try
                                    {
                                        lock (lockObject)
                                        {
                                            if (isMMbuyer) tasUpdate.data.MarketSell = v;
                                            else tasUpdate.data.MarketBuy = v;
                                         
                                            tasUpdate.data.Ask = ask;
                                            tasUpdate.data.Bid = bid;
                                            var dt = long.Parse(tasUpdate.data.EventTime.ToString()).GetFullTime();
                                            tasUpdate.data.EventDate = $"{dt.Day}/{dt.Month} {dt.ToString("HH:mm:ss.ffffff")}";
                                            tasUpdate.data.Id = (int)(++counter);

                                            TasF.Add(tasUpdate);
                                            zz.BuildZigZag(tasUpdate.data.Ask, tasUpdate.data.Bid,
                                                          tasUpdate.data.EventTime, (ulong)tasUpdate.data.Id, TasF);
                                        }
                                    }
                                    catch (NullReferenceException)
                                    {

                                    }

                                }

                                PrevAsk = ask_f;
                                PrevBid = bid_f;
                                PrevTAS = tasUpdate;


                            }
                        }

                        // -----------------------------------------------

                        //-------------------------------------
                    }
                }
            });


        }
        #endregion

        public BinanceFutureClient(IConnectorLogger logger, ManualResetEvent cancelToken, BinanceFutureConnectionModel model) : base(logger, cancelToken, model)
        {
            pingPongManager = new BinancePingPongManager(logger);
            // Створення таймера
            timerEvent = new DispatcherTimer();
            timerEvent.Interval = TimeSpan.FromMilliseconds(10); // 10 мілісекунд

            timerEvent.Tick += OnTick; // Додавання події Tick

            timerEvent.Start(); // Запуск таймера


            Type obj = logger.GetType();
            switch (obj.Name)
            {
                case nameof(TwoLegArbFutureUSD_M):
                    (logger as TwoLegArbFutureUSD_M).fc = this;
                    break;
                case nameof(TradeScalper):
                    (logger as TradeScalper).bfc = this;
                    break;
                case nameof(TradeZigZag):
                    (logger as TradeZigZag).bfc = this;
                    zz = new ZigZagFuture();
                    //(logger as TradeZigZag).zigZagFuture = new ZigZagFuture(this);
                    break;
            }
        }

        internal override decimal GetBalance()
        {
            if ((DateTime.UtcNow - lastBalanceRequestTime).TotalSeconds >= 5)
            {
                lastBalance = GetBalance(BinanceOptionsApp.Models.TradeModel.fullSymbolFuture).Result;
                lastBalanceRequestTime = DateTime.UtcNow;
            }
            return lastBalance;
        }

        public async Task<decimal> GetBalance(string Asset)
        {
            decimal balance = 0;
            try
            {
                if (AccountTradeType == AccountTradeType.USD_M)
                {
                    AccountInfoFuture AIF = await CallAsync<AccountInfoFuture>(EndPoints.AccountInformationFuture, $"recvWindow={recvWindow}");
                    AccountInfo = AIF;
                    AccInfo_UsdUsd_M = AIF;
                    FutureBalances = AIF.Assets;
                    switch (Asset)
                    {
                        case "BTC":
                            balance = decimal.Parse(AIF.Assets.Single(b => b.asset == Asset).AvailableBalance, CultureInfo.InvariantCulture);
                            break;
                        default: //  if we pssed Full symbol
                            balance = decimal.Parse(AIF.AvailableBalance, CultureInfo.InvariantCulture);
                            //decimal.Parse(AIF.Positions.Single(b => b.Symbol == Asset).PositionAmt, CultureInfo.InvariantCulture);
                            break;
                    }
                }
                else if (AccountTradeType == AccountTradeType.COIN_M)
                {
                    AccountInfo_Coin_M_Future details = await CallAsync<AccountInfo_Coin_M_Future>(EndPoints.COIN_MAccountInformationF, $"recvWindow={recvWindow}");
                    Coin_MAccInfo = details;
                    //if (Asset == SubscribeList.First().Key) Asset = "BTC";
                    //var basset = details.Assets.FirstOrDefault(ua => ua.AssetName == "BTC").AvailableBalance;
                    //var bposAmt = details.Positions.FirstOrDefault(ua => ua.Symbol == "BTCUSD_240329").PositionAmt;
                    //var btcasset = details.Assets.FirstOrDefault(ua => ua.AssetName == Asset);
                    switch (Asset)
                    {
                        case "BTC":
                            balance = details.Assets.FirstOrDefault(ua => ua.AssetName == "BTC").AvailableBalance;
                            break;
                        default:
                            var found = details.Positions.FirstOrDefault(ua => ua.Symbol == "BTCUSD_240329");
                            balance = found.PositionAmt;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                //logger.LogError(ViewId + $" Balance Error: {e.Message}");
            }

            return balance;
        }

        #region Order's actions:
        public override OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs)
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
        public override bool OrderDelete(string symbol, string orderId, string origClientOrderId)
        {
            try
            {
                var args = $"symbol={symbol}&orderId={orderId}&origClientOrderId={origClientOrderId}&recvWindow={recvWindow}";

                if (AccountTradeType == AccountTradeType.USD_M)
                {
                    var DeleteResult = CallAsync<BinanceNewOrder>(EndPoints.DeleteOrderFuture, args).Result;

                    if (DeleteResult.status == "CANCELED")
                    {
                        return true;
                    }
                    else return false;
                }
                if (AccountTradeType == AccountTradeType.COIN_M)
                {

                }

                return false;
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return false;
            }
        }
        public override OrderModifyResult Modify(string symbol, string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot)
        {
            try
            {
                bool isSuccess = false;
                string sPrice = newPrice.ToString(CultureInfo.InvariantCulture);
                string sAmount = lot.ToString(CultureInfo.InvariantCulture);
                string sSide = (side == OrderSide.Buy) ? "BUY" : "SELL";

                BinanceNewOrderF ModifyResult = null;
                var args = $"orderId={orderId}&origClientOrderId={origClientOrderId}&symbol={symbol.ToUpper()}" +
                       $"&side={sSide}&quantity={sAmount}&price={sPrice}&recvWindow={recvWindow}";

                DateTime start = DateTime.MinValue, end = DateTime.MinValue;

                if (AccountTradeType == AccountTradeType.COIN_M)
                {
                    BinanceNewOrder o = CallAsync<BinanceNewOrder>(EndPoints.MarginAccountOrderDelete, args).Result;

                    if (o.status == "FILLED")
                    {
                        string Id = o.orderId.ToString();
                        decimal OpenPrice = decimal.Parse(o.price, CultureInfo.InvariantCulture);
                        decimal Volume = decimal.Parse(o.executedQty, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return new OrderModifyResult() { Error = $"Order status = {o.status}" };
                    }

                }
                if (AccountTradeType == AccountTradeType.USD_M)
                {
                    start = DateTime.UtcNow;
                    ModifyResult = CallAsync<BinanceNewOrderF>(EndPoints.OrderModifyFutureUsd_M, args).Result;
                    end = DateTime.UtcNow;

                    if (ModifyResult.Status == "NEW") isSuccess = true;
                }

                if (isSuccess)
                {
                    return new OrderModifyResult()
                    {
                        Lot = ModifyResult.OrigQty,
                        Side = ModifyResult.Side,
                        OpenPrice = ModifyResult.Price,
                        ExecutionTime = end - start
                    };
                }

                return new OrderModifyResult { Error = "Timeout error." };
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new OrderModifyResult { Error = e.Message };
            }
            //return new OrderModifyResult() { Error = "Not implemented" };
        }

        public OrderOpenResult BatchOrder(string symbol, OrderSide side, OrderType otype, decimal lot, decimal price)
        {
            try
            {
                DateTime begin = DateTime.UtcNow;
                OrderInformation order = null;

                string sPrice = price.ToString(CultureInfo.InvariantCulture);
                string sAmount = lot.ToString(CultureInfo.InvariantCulture);
                string sSide = (side == OrderSide.Buy) ? "BUY" : "SELL";

                //var args = $"symbol={symbol}&side={sSide}&type=LIMIT&timeInForce=FOK&quantity={sAmount}&price={sPrice}&recvWindow={recvWindow}";
                var bo = $"'symbol':'{symbol},'side':'{side},'type':'{otype}','timeInForce':'GTC','quantity':'{sAmount}','price':'{sPrice}'";
                bo = $"{{{bo}}}";
                //var args = $"batchOrders=[{bo}]";//&timestamp=1673295895556&signature= 
                var a = "batchOrders =[{ \"symbol\":\"1INCHUSDT\",\"side\":\"BUY\",\"type\":\"STOP\",\"quantity\":30,\"price\":9000,\"timeInForce\":\"GTC\"," +
                                  "\"stopPrice\":9100},{ \"symbol\":\"1INCHUSDT\",\"side\":\"BUY\",\"type\":\"LIMIT\",\"quantity\":30,\"price\":15,\"timeInForce\":\"GTC\"}]}";
                if (AccountTradeType == AccountTradeType.USD_M)
                {
                    BatchOrderFuture o = CallAsync<BatchOrderFuture>(EndPoints.BatchOrderFuture, a).Result;

                    if (o.Status == "FILLED")
                    {
                        //    string Id = o.OrderId.ToString();
                        //    decimal OpenPrice = o.Price;
                        //    decimal Volume = decimal.Parse(o.ExecutedQty, CultureInfo.InvariantCulture);

                        //    order = GetPosition(symbol);
                        //    if (order == null)
                        //    {
                        //        order = new OrderInformation
                        //        {
                        //            Id = symbol,
                        //            Symbol = symbol,
                        //            Side = side,
                        //            OpenTime = DateTime.UtcNow
                        //        };
                        //        AddPosition(order);
                        //    }
                        //    if (order.Side != side)
                        //    {
                        //        RemovePosition(order);
                        //    }
                        //    order.OpenPrice = OpenPrice;
                        //    order.Volume = Volume;
                    }
                    else
                    {
                        return new OrderOpenResult() { Error = $"Order status = {o.Status}" };
                    }
                }

                return new OrderOpenResult() { Error = "Timeout error." };
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new OrderOpenResult() { Error = e.Message };
            }
        }

        public override OrderOpenResult Open(string symbol, decimal price, decimal lot, FillPolicy policy, OrderSide side, int magic, int slippage, int track, OrderType type, int lifetimeMs)
        {
            try
            {
                DateTime begin = DateTime.UtcNow;
                OrderInformation order = null;

                string sPrice = price.ToString(CultureInfo.InvariantCulture);
                string sAmount = lot.ToString(CultureInfo.InvariantCulture);
                string sSide = (side == OrderSide.Buy) ? "BUY" : "SELL";

                var args = "";
                if (type == OrderType.Limit)
                {
                    args = $"symbol={symbol.ToUpper()}&side={sSide}&type=LIMIT&timeInForce={policy.ToString()}&quantity={sAmount}&price={sPrice}&recvWindow={recvWindow}";
                }
                else
                    args = $"symbol={symbol.ToUpper()}&side={sSide}&type=MARKET&quantity={sAmount}&recvWindow={recvWindow}";

                if (AccountTradeType == AccountTradeType.USD_M)
                {
                    BinanceNewOrderF o = CallAsync<BinanceNewOrderF>(EndPoints.NewOrderFuture, args).Result;

                    if (o.Status == "FILLED")
                    {
                        PlacedOrdersUsd_M.Add(o);
                        string Id = o.OrderId.ToString();
                        decimal OpenPrice = o.Price;
                        decimal Volume = decimal.Parse(o.ExecutedQty, CultureInfo.InvariantCulture);

                        order = GetPosition(symbol);
                        if (order == null)
                        {
                            order = new OrderInformation
                            {
                                Id = symbol,
                                Symbol = symbol,
                                Side = side,
                                OpenTime = DateTime.UtcNow
                            };
                            AddPosition(order);
                        }
                        if (order.Side != side)
                        {
                            RemovePosition(order);
                        }
                        order.OpenPrice = OpenPrice;
                        order.Volume = Volume;
                    }
                    if (o.Status == "NEW")
                    {
                        PlacedOrdersUsd_M.Add(o);
                        string Id = o.OrderId.ToString();
                        decimal OpenPrice = o.Price;
                        decimal Volume = decimal.Parse(o.ExecutedQty, CultureInfo.InvariantCulture);

                        order = GetPosition(symbol);
                        if (order == null)
                        {
                            order = new OrderInformation
                            {
                                Id = symbol,
                                Symbol = symbol,
                                Side = side,
                                OpenTime = DateTime.UtcNow
                            };
                            AddPosition(order);
                        }
                        if (order.Side != side)
                        {
                            RemovePosition(order);
                        }
                        order.OpenPrice = OpenPrice;
                        order.Volume = Volume;
                    }
                    else
                    {
                        return new OrderOpenResult() { Error = $"Order status = {o.Status}" };
                    }

                }
                if (AccountTradeType == AccountTradeType.COIN_M)
                {
                    COIN_M_NewOrder o = CallAsync<COIN_M_NewOrder>(EndPoints.Coin_M_NewOrder, args).Result;

                    if (o.status == "NEW")
                    {
                        PlacedOrdersCoin_M.Add(o);
                        string Id = o.orderId.ToString();
                        decimal OpenPrice = o.price;
                        decimal Volume = decimal.Parse(o.executedQty, CultureInfo.InvariantCulture);

                        order = GetPosition(symbol);
                        if (order == null)
                        {
                            order = new OrderInformation
                            {
                                Id = symbol,
                                Symbol = symbol,
                                Side = side,
                                OpenTime = DateTime.UtcNow
                            };
                            AddPosition(order);
                        }
                        if (order.Side != side)
                        {
                            RemovePosition(order);
                        }
                        order.OpenPrice = OpenPrice;
                        order.Volume = Volume;
                    }
                }

                if (order != null)
                {
                    return new OrderOpenResult()
                    {
                        Id = order.Id,
                        ExecutionTime = DateTime.UtcNow - begin,
                        OpenPrice = order.OpenPrice
                    };
                }

                return new OrderOpenResult() { Error = "Timeout error." };
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new OrderOpenResult() { Error = e.Message };
            }
        }
        #endregion

        public override void Subscribe(string symbol, string id, string algo = "")
        {
            this.symbol = symbol;
            var quote = new TickEventArgs
            {
                Symbol = symbol,
                SymbolId = id
            };

            WebSocketData data = new WebSocketData
            {
                Active = true,
                quote = quote
            };

            if (SubscribeList.ContainsKey(quote.Symbol)) SubscribeList[quote.Symbol] = data;
            else SubscribeList.Add(quote.Symbol, data);
            // Check Subscribe Websoket's Events:
            switch (algo)
            {
                case "ZigZag":
                    //  SubscribeBookTickerStream(symbol);
                    ConnectToDepthStream(symbol);
                    SubscribePartialDepthStream(quote.Symbol);
                    SubscribeToAggTradeStream(quote.Symbol);
                    break;
                case "Scalper":
                    SubscribePartialDepthStream(quote.Symbol);
                    SubscribeToAggTradeStream(quote.Symbol);
                    break;
                case "TwoLegArbitrage":
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
                case "LatencyArbitrage":
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
                case "COIN_M":
                    PartialDepth_COIN_M(quote.Symbol);
                    break;
                case "USD_M":
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
                default:
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
            }
        }

        public override void Unsubscribe(string symbol, string id)
        {
            if (SubscribeList.ContainsKey(symbol))
            {
                SubscribeList[symbol].Active = false;
                SubscribeList[symbol].webSocket.Close();
                SubscribeList.Remove(symbol);
            }
        }

        public override void Start()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", Key);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _IsLoggedIn = true;

            StartUserDataStream();

            //// Створення таймера
            //timerEvent = new DispatcherTimer();
            //timerEvent.Interval = TimeSpan.FromMilliseconds(100); // 1 мілісекундa

            //timerEvent.Tick += OnTick; // Додавання події Tick

            //timerEvent.Start(); // Запуск таймера
        }

        private async void StartUserDataStream()
        {
            try
            {
                BinanceEndPoint ep = (AccountTradeType == AccountTradeType.USD_M)
                    ? EndPoints.SpotCreateListenKeyFuture : EndPoints.MarginCreateListenKey;
                BinanceListenKey o = await CallAsync<BinanceListenKey>(ep);
                if (!string.IsNullOrEmpty(o.listenKey))
                {
                    OnLoggedIn();
                }
                else
                {
                    // logger.LogError(ViewId + $" listenKey is null");
                }

                try
                {
                    decimal balance = GetBalance();
                    logger.LogInfo(ViewId + $" {AccountTradeType} Balance: {balance} USDT");
                }
                catch (Exception e)
                {
                    logger.LogError(ViewId + $" Margin Balance Error: {e.Message}");
                }
            }
            catch (Exception e)
            {
                logger.LogError(ViewId + $" {e.Message}");
            }
        }

        public override void Stop(bool wait)
        {
            _IsLoggedIn = false;
            OnLoggedOut();
        }

        #region API
        public async Task<T> CallAsync<T>(BinanceEndPoint endpoint, string parameters = null, bool firstCall = true)
        {
            string finalEndpoint = endpoint.Value + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (endpoint.isSigned)
            {
                string p = parameters + (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + GenerateTimeStamp(DateTime.Now.ToUniversalTime());
                var signature = GenerateSignature(Secret, p);
                finalEndpoint = $"{endpoint.Value}?{p}&signature={signature}";
            }

            string urlPath = AccountTradeType == AccountTradeType.USD_M ? "https://fapi.binance.com" + finalEndpoint : urlPath = "https://dapi.binance.com" + finalEndpoint;
            var path = new Uri(urlPath);

            HttpResponseMessage response;
            if (endpoint.Method == "POST")
                response = await _httpClient.PostAsync(path, null);
            else if (endpoint.Method == "GET")
                response = await _httpClient.GetAsync(path);
            else if (endpoint.Method == "PUT")
                response = await _httpClient.PutAsync(path, null);
            else if (endpoint.Method == "DELETE")
                response = await _httpClient.DeleteAsync(path);
            else
                throw new Exception($"Unknown type of request: endpoint.Method = \"{endpoint.Method}\"");

            if (response.IsSuccessStatusCode)
            {
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<T>(result);
            }

            if (response.StatusCode == HttpStatusCode.GatewayTimeout)
            {
                throw new Exception("Api Request Timeout.");
            }

            var e = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //"{\"code\":-1021,\"msg\":\"Timestamp for this request was 1000ms ahead of the server's time.\"}"
            var eCode = 0;
            string eMsg = "";
            if (e.IsValidJson())
            {
                try
                {
                    var i = JObject.Parse(e);

                    eCode = i["code"]?.Value<int>() ?? 0;
                    eMsg = i["msg"]?.Value<string>();
                }
                catch { }
            }

            if (firstCall && eCode == -1021)
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long clientTime = (long)(DateTime.Now.ToUniversalTime() - epoch).TotalMilliseconds;
                BinanceServerTime sTime = await CallAsync<BinanceServerTime>(EndPoints.CheckServerTimeFuture);
                dTime = clientTime - sTime.serverTime;

                long clientTime2 = (long)(DateTime.Now.ToUniversalTime() - epoch).TotalMilliseconds;
                if ((clientTime2 - clientTime) < 1000)
                    return await CallAsync<T>(endpoint, parameters, false);
                else
                    throw new Exception(string.Format("Api Error Code: {0} Message: {1}", eCode, eMsg));
            }
            else
                throw new Exception(string.Format("Api Error Code: {0} Message: {1}", eCode, eMsg));
        }

        public string GenerateTimeStamp(DateTime baseDateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return ((long)(baseDateTime.ToUniversalTime() - epoch).TotalMilliseconds - dTime).ToString();
        }
        public string GenerateSignature(string apiSecret, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiSecret);
            string stringHash;
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                stringHash = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            return stringHash;
        }
        #endregion

        #region Binance future's events [API streams][WebSocket]:
        #region Fields:
        public static ConcurrentDictionary<decimal, MarketDepthUpdateFuture> marketDepthList { get; set; }
            = new ConcurrentDictionary<decimal, MarketDepthUpdateFuture>();

        public static ConcurrentDictionary<long, BookTickerF> BookTickerFutureDictionary = new ConcurrentDictionary<long, BookTickerF>();

        public static List<BookTickerF> BookTickerFuture { get; set; } = new List<BookTickerF>();
        private BookTickerF Pre_BookTickerFuture { get; set; }
        private BookTickerF Pre2_BookTickerFuture { get; set; }
        private BookTickerF Pre3_BookTickerFuture { get; set; }

        public List<AggTradeFuture> TasF = new List<AggTradeFuture>();
        public static List<AggTradeFuture> TasRawF = new List<AggTradeFuture>();
        public List<AggTradeFuture> TasCopyF = new List<AggTradeFuture>();
        private AggTradeFuture tasF;
        private ZigZagFuture zz;
        private ulong counter = 0;
        private double standardDeviationBuy = 0;
        private double standardDeviationSell = 0;
        private decimal meanBuy = 0, meanSell = 0;
        private AggTradeFuture PrevTAS { get; set; }
        private decimal PrevAsk { get; set; }
        private decimal PrevBid { get; set; }
        public decimal LastAskF { get; set; }
        public decimal LastBidF { get; set; }
        private QuoteAskBidFuture qAB = new QuoteAskBidFuture();

        private object lockObject = new object();
        private static ConcurrentDictionary<decimal, MarketDepthUpdateFuture> marketDepthUpdate_Cpy
            = new ConcurrentDictionary<decimal, MarketDepthUpdateFuture>();

        public static decimal QuoteAsk { get; set; }
        public static decimal QuoteBid { get; set; }
        public decimal ask_f { get; set; }
        public decimal bid_f { get; set; }

        public decimal BestAskFuture { get; set; }
        public decimal BestBidFuture { get; set; }
        // Define an object for locking
        #endregion

        #region Get Market Depth [Http request] [https://fapi.binance.com/fapi/v1/depth?symbol=btcusdt&limit=1000]:
        public static DateTime lastTimeUpdDepth5000 = DateTime.MinValue;
        public async void CallMarketDepth(string symbol = "BTCUSDT")
        {
            string depthEndpoint = $"https://fapi.binance.com/fapi/v1/depth?symbol={symbol}&limit=1000";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(depthEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        //lastTimeUpdDepth5000 = DateTime.Now;
                        string responseContent = await response.Content.ReadAsStringAsync();

                        var depthUpdate = JsonConvert.DeserializeObject<DepthHttpReqFuture>(responseContent);
                        marketDepthList = new ConcurrentDictionary<decimal, MarketDepthUpdateFuture>();
                        foreach (var ask in depthUpdate.asks)
                        {
                            try
                            {
                                var key = decimal.Parse(ask[0], CultureInfo.InvariantCulture);
                                marketDepthList[key] =
                                    new MarketDepthUpdateFuture { Type = 1, Volume = decimal.Parse(ask[1], CultureInfo.InvariantCulture), LastTimeUpd = DateTime.Now.ToString("mm:ss.fff") };
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"[CallMarketDepth](asks): Exception message:\n{ex.Message}\nStack Trace:\n{ex.StackTrace}");
                            }
                        }
                        foreach (var bid in depthUpdate.bids)
                        {
                            try
                            {
                                var key = decimal.Parse(bid[0], CultureInfo.InvariantCulture);
                                marketDepthList[key] =
                                     new MarketDepthUpdateFuture { Type = 2, Volume = decimal.Parse(bid[1], CultureInfo.InvariantCulture), LastTimeUpd = DateTime.Now.ToString("mm:ss.fff") };
                            }
                            catch (Exception ex)
                            {
                                logger.LogError($"[CallMarketDepth](bids): Exception message:\n{ex.Message}\nStack Trace:\n{ex.StackTrace}");
                            }
                        }
                    }
                    else logger.LogError($"[CallMarketDepth)]: Failed to fetch data. Status code: {response.StatusCode}");
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError($"Request exception: {ex.Message}");
                }
            }
            await Task.Delay(0);
        }
        #endregion

        #region Get Market Depth [WebSoket] [Up to date Depth] [wss://stream.binance.com:9443/ws/btcusdt@depth]:
        /// <summary>
        /// MARKET DEPTH [UPDATE GENERAL DCTIONARY] [via WebSocket]:
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public void ConnectToDepthStream(string symbol = "BTCUSDT")
        {
            depthWS = new WebSocket4Net.WebSocket($"wss://fstream.binance.com/stream?streams={symbol.ToLower()}@depth@500ms");

            depthWS.Opened += (sender, e) => logger.LogInfo("WebSocket connected! Start @depth [Future] process...");

            depthWS.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            depthWS.EnableAutoSendPing = true;
            depthWS.AutoSendPingInterval = 180;
            // ++++++++++++++++++++++++++++++++++++++++++++++


            //+++++++++++++++++++++++++++++++++++++++++++++++
            depthWS.MessageReceived += (sender, e) =>
            {
                var depth = JsonConvert.DeserializeObject<DepthFuture>(e.Message);
                foreach (var ask in depth.data.Asks)
                    UpdDepthData(ask[0], ask[1], 1);
                foreach (var bid in depth.data.Bids)
                    UpdDepthData(bid[0], bid[1], 2);
                // Отримуємо копію стакану:
                marketDepthUpdate_Cpy = GetSafeCopyOfMarketDepthList(symbol);
                var time = DateTime.Now.GetTotalMilliseconds() - startDepth;
                ///if (pingPongManager == null && depthWS.State == WebSocketState.Open) 
                //pingPongManager.StartPingPong(depthWS);
                //depthWS.Send("ping");
            };

            depthWS.Error += (sender, e) =>
            {
                logger.LogError($"WebSocket [ConnectToDepthStream()] [future] error: {e.Exception.Message}");
            };

            depthWS.Closed += (s, e) =>
            {
                if (isCloseMDCustom)
                {
                    logger.LogInfo("MarketDepth was closed by Custom!");
                    depthWS.Dispose();
                }
                else
                {
                    Task.Delay(5000).ContinueWith(_ =>
                    {
                        ReconectCount++;
                        if (ReconectCount > 5)
                        {
                            // Can not open WebSocket;
                            return;
                        }
                        logger.LogInfo(ViewId + " Reconect ConnectToDepthStream() to WebSocket #");

                        // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                        if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                        timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                        logger.LogWarning($"WebSocket [ConnectToDepthStream()] [Closed] [future] and reconnect again [during Task.Delay]..");
                        ConnectToDepthStream();
                    });
                }
                //logger.LogWarning($"WebSocket [ConnectToDepthStream()] [Closed] [future] and reconnect again..");
                //ConnectToDepthStream();
            };
            depthWS.Open();
        }

        /// <summary>
        /// Copy general array marketDepthUpdate:
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>completed copy</returns>
        public ConcurrentDictionary<decimal, MarketDepthUpdateFuture> GetSafeCopyOfMarketDepthList(string symbol)
        {
            ConcurrentDictionary<decimal, MarketDepthUpdateFuture> copyOfMarketDepthList
                = new ConcurrentDictionary<decimal, MarketDepthUpdateFuture>();

            lock (lockObject)
            {
                foreach (var kvp in marketDepthList)
                {
                    copyOfMarketDepthList.TryAdd(kvp.Key, kvp.Value.DeepCopy());
                }

            }

            return copyOfMarketDepthList;
        }
        #endregion

        #region Partial Depth:
        public static List<List<string>> BidsP { get; set; }
        public static List<List<string>> AsksP { get; set; }
        public static List<QuoteFuture> Quotes = new List<QuoteFuture>();
        // <symbol>@bookTicker
        public void SubscribePartialDepthStream(string symbol = "BTCUSDT")
        {
            var webSocket = new WebSocket4Net.WebSocket($"wss://fstream.binance.com/stream?streams={symbol.ToLower()}@depth20@500ms");

            webSocket.Opened += (sender, e) => logger.LogInfo("WebSocket connected! Start Partial @depth20 [Future] process...");

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                var depth = JsonConvert.DeserializeObject<PartialDepthFuture>(e.Message);
                //var asks = depth.Data.Asks.OrderBy(p => p[0]).Select(o => o[0]).ToList();
                //var bids = depth.Data.Bids.OrderByDescending(p => p[0]).Select(o => o[0]).ToList();
                //BestAskFuture = decimal.Parse(depth.Data.Asks.Select(o => o[0]).First(), CultureInfo.InvariantCulture);
                //BestBidFuture = decimal.Parse(depth.Data.Bids.Select(o => o[0]).First(), CultureInfo.InvariantCulture);
                OnTick(new TickEventArgs
                {
                    Asks = AsksP = depth.Data.Asks,
                    Bids = BidsP = depth.Data.Bids,
                    Ask = QuoteAsk = decimal.Parse(depth.Data.Asks.Select(o => o[0]).First(), CultureInfo.InvariantCulture),//BestAskFuture,
                    Bid = QuoteBid = decimal.Parse(depth.Data.Bids.Select(o => o[0]).First(), CultureInfo.InvariantCulture),//BestBidFuture,
                    Symbol = symbol
                });
            };

            webSocket.Error += (sender, e) => logger.LogError($"WebSocket [SubscribePartialDepthStream()] [future] error: {e.Exception.Message}");
            webSocket.Closed += (s, e) =>
            {
                Task.Delay(5000).ContinueWith(_ =>
                    {
                        ReconectCount++;
                        if (ReconectCount > 5)
                        {
                            // Can not open WebSocket;
                            return;
                        }
                        logger.LogInfo(ViewId + " Reconect to WebSocket #");

                        // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                        if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                        timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                        logger.LogWarning($"WebSocket [SubscribePartialDepthStream()] [Closed] [future] and reconnect again [during Task.Delay]..");
                        ConnectToDepthStream();
                    });
                //logger.LogWarning($"WebSocket [SubscribePartialDepthStream()] [Closed] [future] and reconnect again..");
                //ConnectToDepthStream();
            };
            webSocket.Open();
        }
        #endregion

        #region Get BookTicker for Quotes [wss://fstream.binance.com/stream?streams=BTCUSDT@bookTicker]:
        long startTime2 = 0;
        public void SubscribeBookTickerStream(string symbol = "BTCUSDT")
        {

            bookTikerWS = new WebSocket4Net.WebSocket($"wss://fstream.binance.com/stream?streams={symbol.ToLower()}@bookTicker");
            // bookTikerWS.
            bookTikerWS.Opened += (sender, e) => logger.LogInfo("WebSocket connected! Start @bookTicker [Future] process...");

            bookTikerWS.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            bookTikerWS.NoDelay = true;
            bookTikerWS.EnableAutoSendPing = true;
            bookTikerWS.AutoSendPingInterval = 180;

            bookTikerWS.MessageReceived += (sender, e) =>
            {
                var bt = JsonConvert.DeserializeObject<BookTickerF>(e.Message);
                bt.Data.SynchronizationTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                AddToDictionary(bt);
                BookTickerFuture.Add(bt);
                //-----------------------------------------------
            };

            bookTikerWS.Error += (sender, e) =>
            {
                if (e.Exception.Message.Contains("429"))
                {
                    logger.LogError("!!!!!!!!!!!!!!!!!! We have reached the limit. WARNING! 429 !!!!!!!!!!!!!!!!!!!!");
                    // call all closed websocket ...
                }
                logger.LogError($"WebSocket [SubscribeBookTickerStream()] [future] error: {e.Exception.Message}");
            };

            bookTikerWS.Closed += (s, e) =>
            {
                if (isCloseBTCustom)
                {
                    logger.LogInfo("BookTicker was closed by Custom!");
                    bookTikerWS.Dispose();
                }
                else
                {
                    Task.Delay(5000).ContinueWith(_ =>
                    {
                        ReconectCount++;
                        if (ReconectCount > 5)
                        {
                            // Can not open WebSocket;
                            return;
                        }
                        logger.LogInfo(ViewId + " Reconect SubscribeBookTickerStream WebSocket #");

                        // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                        if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                        timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                        logger.LogWarning($"WebSocket [SubscribeBookTickerStream()] [Closed] [future] and reconnect again [during Task.Delay]..");
                        SubscribeBookTickerStream(symbol);
                    });
                }
            };
            bookTikerWS.Open();
            Task.Delay(10);
        }

        public static void AddToDictionary(BookTickerF model)
        {
            if (BookTickerFutureDictionary.TryAdd(model.Data.EventTime, model))
            {
                //  var sorted = BookTickerFutureDictionary.OrderBy(t => t.Key);  // Відсортувати словник за зростанням ключа (EventTime)

                //   var sortedDictionary = new ConcurrentDictionary<long, BookTickerF>(sorted);  // Створити новий відсортований словник

                //  BookTickerFutureDictionary = sortedDictionary; // Замінити оригінальний словник відсортованим
            }

        }

        long lastSearchTime = 0; int lastIndexF = 0;
        public BookTickerF FindModelByTime(long targetTime)
        {
            var CopyBook = BookTickerFuture;

            lock (lockObject)
            {
                BookTickerF mod;
                // var foundModel = CopyBook.FirstOrDefault(model => model.SynchronizationTime >= targetTime && model.SynchronizationTime >= lastSearchTime);
                var foundModelInd = CopyBook.FindIndex(lastIndexF, model => model.Data.SynchronizationTime >= targetTime);
                if (foundModelInd > 0)
                {
                    mod = CopyBook[foundModelInd];
                    //  lastSearchTime = foundModel.SynchronizationTime;
                    lastIndexF = foundModelInd;
                    return mod;
                }
                else return null;
                // return foundModel;
            }
        }

        public BookTickerF FindModelByTime2(long targetTime)
        {
            var foundModel = BookTickerFutureDictionary
                .Where(pair => pair.Value.Data.EventTime >= targetTime)
                .OrderBy(pair => pair.Value.Data.EventTime)
                .Select(pair => pair.Value)
                .FirstOrDefault();

            return foundModel;
        }

        public static void RemoveOlderThanSynchronizationTime(long synchronizationTime)
        {
            var keysToRemove = BookTickerFutureDictionary// Отримати список ключів, які потрібно видалити
                .Where(pair => pair.Value.Data.EventTime < synchronizationTime - 3000)
                .Select(pair => pair.Key)
                .ToList();

            foreach (var key in keysToRemove)// Видалити об'єкти за визначеними ключами
            {
                BookTickerFutureDictionary.TryRemove(key, out _);
            }
        }
        #endregion

        #region Time and Sales [WebSoket] [wss://fstream.binance.com:9443/ws/btcusdt@aggTrade]:
        // Створення словника Plot
        Dictionary<decimal, Plot> TradeLevelPlot = new Dictionary<decimal, Plot>();
        private BookTickerF PreMod { get; set; }
        // Створення нового таймера Stopwatch
        Stopwatch stopwatch = new Stopwatch();
        public async void SubscribeToAggTradeStream(string symbol = "btcusdt")
        {
            aggTrade = new WebSocket($"wss://fstream.binance.com/stream?streams={symbol.ToLower()}@aggTrade");

            aggTrade.Opened += (sender, e) =>
            {
                logger.LogInfo("Lenta has started...Start SubscribeToAggTradeStream @aggTrade [future] process...");
            };

            aggTrade.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            aggTrade.EnableAutoSendPing = true;
            aggTrade.AutoSendPingInterval = 180;

            long TimeTrade;
            var mod = TasRawF.Count > 0 && TasRawF != null ? TasRawF.Last() : null;

            //------------------------------------------
            aggTrade.Error += (sender, e) =>
            {
                logger.LogError(e.Exception.Message);
            };

            int lastIndexF = 0; long startTime = 0;

            // webSocket.MessageReceived += (sender, e) => //// TAS
            aggTrade.MessageReceived += async (sender, e) =>
            {
                var tasUpdateR = JsonConvert.DeserializeObject<AggTradeFuture>(e.Message);
                if (startTime == 0) startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                //==================================
                TasRawF.Add(tasUpdateR);
                TasCopyF = TasRawF;
                //==================================
                var p = tasUpdateR.data.Price;
                var buy = tasUpdateR.data.IsMarketMaker;
                //++++++++++++++++++++++++++++++++++++++++++


                // Додавання значень до словника за ключем Price
                decimal price1 = 100.50m;
                TradeLevelPlot.Add(price1, new Plot { MarketBuy = 10, MarketSell = 15, Price = price1 });

                decimal price2 = 105.20m;
                TradeLevelPlot.Add(price2, new Plot { MarketBuy = 8, MarketSell = 12, Price = price2 });

                // Приклад витягування значень зі словника за ключем
                decimal keyToRetrieve = 100.50m;
                if (TradeLevelPlot.ContainsKey(keyToRetrieve))
                {
                    Plot plot = TradeLevelPlot[keyToRetrieve];
                    Console.WriteLine($"Для Price {keyToRetrieve}: MarketBuy = {plot.MarketBuy}, MarketSell = {plot.MarketSell}");
                }

                //++++++++++++++++++++++++++++++++++++++++++
                // Оновлення значень за ключем Price
                decimal keyToUpdate = 100.50m;
                if (TradeLevelPlot.ContainsKey(keyToUpdate))
                {
                    // Отримання об'єкта Plot за ключем
                    Plot plotToUpdate = TradeLevelPlot[keyToUpdate];

                    // Оновлення значень MarketSell та MarketBuy
                    plotToUpdate.MarketSell = 20;
                    plotToUpdate.MarketBuy = 12;

                    Console.WriteLine($"Значення MarketSell і MarketBuy для Price {keyToUpdate} оновлені.");
                }
                //++++++++++++++++++++++++++++++++++++++++++

                //==================================

            };

            aggTrade.Closed += (s, e) =>
            {
                if (isCloseAggTrdCustom)
                {
                    logger.LogInfo("AggTrade was closed by Custom!");
                    aggTrade.Dispose();
                }
                else
                {
                    Task.Delay(5000).ContinueWith(_ =>
                        {
                            ReconectCount++;
                            if (ReconectCount > 5)
                            {
                                // Can not open WebSocket;
                                return;
                            }
                            logger.LogInfo(ViewId + " Reconect to WebSocket #");

                            // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                            if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                            timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                            logger.LogError($"WebSocket [SubscribeToAggTradeStream()] [Closed] [future] and reconnect again [with Task.Delay]..");
                            SubscribeToAggTradeStream();
                        });
                }
            };
            aggTrade.Open();
        }
        #endregion

        #region Auxiliary methods:        
        private void UpdDepthData(string price, string volume, int type)
        {
            var key = decimal.Parse(price, CultureInfo.InvariantCulture);
            if (marketDepthList.ContainsKey(key))
            {
                if (volume == "0.000")
                    marketDepthList.TryRemove(key, out MarketDepthUpdateFuture val);
                else
                {
                    try
                    {
                        var found = marketDepthList[key];
                        if (found != null)
                        {
                            marketDepthList[key] = new MarketDepthUpdateFuture
                            {
                                Volume = decimal.Parse(volume, CultureInfo.InvariantCulture),
                                Type = type,
                                LastTimeUpd = DateTime.Now.ToString("mm:ss.fff")
                            };
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (!marketDepthList.ContainsKey(key))
            {
                try
                {
                    if (volume == "0.000")
                    {
                    }
                    else
                    {
                        marketDepthList.TryAdd(key, new MarketDepthUpdateFuture
                        { Volume = decimal.Parse(volume, CultureInfo.InvariantCulture), Type = type, LastTimeUpd = DateTime.Now.ToString("mm:ss.fff") });
                    }
                }
                catch (Exception ex)
                {
                }
            }

        }

        public static void FunStD(decimal SellPrice, decimal BuyPrice, decimal Ask, decimal Bid)
        {
            //int i = (int)(SellPrice * 100);
            //int ii = (int)(BuyPrice * 100);
            //int iAsk = (int)(Ask * 100);
            //int iBid = (int)(Bid * 100);

            //if (iAsk >= 0 && iAsk < marketDepthList.Count && i >= 0 && i < marketDepthList.Count && i > iAsk)
            //{
            //    var volumeRange = marketDepthList
            //        .Where(entry => int.Parse(entry.Key) >= iAsk && int.Parse(entry.Key) <= i && entry.Value.Type == 1 && entry.Value.Volume > 0)
            //        .Select(entry => entry.Value.Volume);

            //    if (volumeRange.Any())
            //    {
            //        meanSell = volumeRange.Average();
            //        double squaredDifferencesSum = volumeRange.Sum(volume => Math.Pow((double)(volume - meanSell), 2));
            //        double variance = squaredDifferencesSum / volumeRange.Count();
            //        standardDeviationSell = Math.Sqrt(variance);
            //    }
            //    else
            //    {
            //        // В діапазоні немає елементів
            //    }
            //}
            //else
            //{
            //    // Індекси виходять за межі масиву
            //}

            //if (iBid >= 0 && iBid < marketDepthList.Count && ii >= 0 && ii < marketDepthList.Count && iBid > ii)
            //{
            //    var volumeRange = marketDepthList
            //        .Where(entry => int.Parse(entry.Key) >= ii && int.Parse(entry.Key) <= iBid && entry.Value.Type == 2 && entry.Value.Volume > 0)
            //        .Select(entry => entry.Value.Volume);

            //    if (volumeRange.Any())
            //    {
            //        meanBuy = volumeRange.Average();
            //        double squaredDifferencesSum = volumeRange.Sum(volume => Math.Pow((double)(volume - meanBuy), 2));
            //        double variance = squaredDifferencesSum / volumeRange.Count();
            //        standardDeviationBuy = Math.Sqrt(variance);
            //    }
            //    else
            //    {
            //        // В діапазоні немає елементів
            //    }
            //}
            //else
            //{
            //    // Індекси виходять за межі масиву
            //}
        }

        public static void AssignRankToRange(decimal SellPrice, decimal BuyPrice, decimal Ask, decimal Bid)
        {
            int iSellPrice = (int)(SellPrice * 100);
            int iBuyPrice = (int)(BuyPrice * 100);
            int iAsk = (int)(Ask * 100);
            int iBid = (int)(Bid * 100);

            // if (iAsk >= 0 && iAsk < marketDepthList.Count && iSellPrice >= 0 && iSellPrice < marketDepthList.Count && iSellPrice > iAsk)
            if (iAsk >= 0 && iSellPrice >= 0 && iSellPrice > iAsk)
            {
                var askVolumeRange = marketDepthList
                    .Skip(iSellPrice)//iAsk
                    .Take(1000) //iSellPrice - iAsk
                                // .Where(entry => entry.Value != null && int.TryParse(entry.Key, out int entryType) && entryType == 1 && entry.Value.Volume > 0)
                    .Where(entry => entry.Value != null)
                    .Select(entry => entry.Value.Volume)
                    .ToList();

                var sortedAskVolume = askVolumeRange.ToList();
                sortedAskVolume.Sort((a, b) => b.CompareTo(a));

                var askVolumeRangeList = askVolumeRange.ToList();
                int Sz = askVolumeRangeList.Count;
                for (int i = 0; i < askVolumeRangeList.Count; i++)
                {
                    int rank = sortedAskVolume.IndexOf(askVolumeRangeList[i]) + 1;
                    int index = iSellPrice + i;//(iSellPrice + i).ToString(); //

                    if (index >= 0 && index < marketDepthList.Count)
                    {
                        //marketDepthList[index.ToString()].RankAsk = rank;
                    }
                }
            }

            // if (iBid >= 0 && iBid < marketDepthList.Count && iBuyPrice >= 0 && iBuyPrice < marketDepthList.Count && iBid > iBuyPrice)
            if (iBid >= 0 && iBuyPrice >= 0 && iBid > iBuyPrice)
            {
                //var bidVolumeRange = marketDepthList
                //    .Skip(iBuyPrice)
                //    .Take(iBid - iBuyPrice)
                //    .Where(entry => entry.Value != null && int.TryParse(entry.Key, out int entryType) && entryType == 2 && entry.Value.Volume > 0)
                //    .Select(entry => entry.Value.Volume)
                //    .ToList();

                //var sortedBidVolume = bidVolumeRange.ToList();
                //sortedBidVolume.Sort((a, b) => b.CompareTo(a));

                //var bidVolumeRangeList = bidVolumeRange.ToList();
                //for (int i = 0; i < bidVolumeRangeList.Count; i++)
                //{
                //    int rank = sortedBidVolume.IndexOf(bidVolumeRangeList[i]) + 1;
                //    int index = iBuyPrice + i;
                //    if (index >= 0 && index < marketDepthList.Count)
                //    {
                //        marketDepthList[index.ToString()].RankBid = rank;
                //    }
                //}
            }
        }
        #endregion

        #region My func:
        public async Task MyFunc()
        {
            await Task.Delay(0);
            if (LastAskF != 0 && LastBidF != 0)
            {
                decimal SumA = 0, SumB = 0;

                var a = LastAskF;
                var b = LastBidF;
                var start = LastAskF + 1.2m;
                for (decimal i = start; i > LastBidF - 1.2m; i -= 0.01m)
                {
                    if (marketDepthList.ContainsKey(i))
                    {
                        var elem = marketDepthList[i];
                        if (elem != null)
                        {
                            if (elem.Type == 1 && elem.Volume > 0)
                            {
                                SumA += elem.Volume;

                                //  ForegroundColor = ConsoleColor.Red;
                                //WriteLine(" Ask: {0:F2} | Vol: {1}", i, marketDepthList[((int)price).ToString()].Volume);
                                //ResetColor();
                            }
                            else if (elem.Type == 2 && elem.Volume > 0)
                            {
                                SumB += elem.Volume;
                                // ForegroundColor = ConsoleColor.Green;
                                //WriteLine(" Bid: {0:F2} | Vol: {1}", i, marketDepthList[((int)price).ToString()].Volume);
                                //ResetColor();
                            }
                        }
                    }
                }
                //zz.SumAsk = SumA; zz.SumBid = SumB;
            }
        }
        #endregion

        #region Synchronization_Level_2()[]:

        static void Synchronization_Level_2()
        {




        }








        #endregion

        #region FunDepthProba1 & FunDepthProba2:
        public int FunDepthProba1(int Tip)
        {
            int res = 0;

            if (LastAskF != 0 && LastBidF != 0)
            {
                //decimal SumA = 0, SumB = 0;
                int R_A = 0, R_B = 0;
                decimal SuppP = 0, SuppV = 0, ResisP = 0, ResisV = 0;

                var a = LastAskF;
                var b = LastBidF;
                var start = LastAskF + 100.01m;
                for (decimal i = start; i > LastBidF - 100.01m; i -= 0.01m)
                {
                    if (marketDepthList.ContainsKey(i))
                    {
                        var elem = marketDepthList[i];
                        if (elem != null)
                        {
                            if (Tip == 1 && elem.Type == 1 && elem.Volume > 0)
                            {
                                //SumA += elem.Volume;
                                if (elem.RankAsk < R_A || R_A == 0)
                                {
                                    R_A = elem.RankAsk;
                                    ResisP = i;
                                    ResisV = elem.Volume;
                                }


                            }
                            else if (Tip == 2 && elem.Type == 2 && elem.Volume > 0)
                            {
                                //SumB += elem.Volume;
                                if (elem.RankBid < R_B || R_B == 0)
                                {
                                    R_B = elem.RankBid;
                                    SuppP = i;
                                    SuppV = elem.Volume;
                                }

                            }
                        }
                    }
                }
                //if (Tip == 1) { zz.ResistancePrice = ResisP; zz.ResistanceVol = ResisV; res = R_A; }
                //if (Tip == 2) { zz.SupportPrice = SuppP; zz.SupportVol = SuppV; res = R_B; }

            }
            return (res);
        }

        public static int FunDepthProba2(int Tip)
        {
            int res = 0;

            //if (LastAsk != 0 && LastBid != 0)
            //{
            //    int R_A = 0, R_B = 0;
            //    decimal SuppP = 0, SuppV = 0, ResisP = 0, ResisV = 0;

            //    var a = LastAsk;
            //    var b = LastBid;
            //    var start = LastAsk + 100.01m;

            //    foreach (var kvp in marketDepthList.Reverse())
            //    {
            //        decimal i = kvp.Key; // конвертація стрінгу в decimal
            //        var elem = kvp.Value;

            //        if (Tip == 1 && elem.Type == 1 && elem.Volume > 0)
            //        {
            //            if (elem.RankAsk < R_A || R_A == 0)
            //            {
            //                R_A = elem.RankAsk;
            //                ResisP = i;
            //                ResisV = elem.Volume;
            //            }
            //        }
            //        else if (Tip == 2 && elem.Type == 2 && elem.Volume > 0)
            //        {
            //            if (elem.RankBid < R_B || R_B == 0)
            //            {
            //                R_B = elem.RankBid;
            //                SuppP = i;
            //                SuppV = elem.Volume;
            //            }
            //        }
            //    }

            //    if (Tip == 1) { zz.ResistancePrice = ResisP; zz.ResistanceVol = ResisV; res = R_A; }
            //    if (Tip == 2) { zz.SupportPrice = SuppP; zz.SupportVol = SuppV; res = R_B; }
            //}

            return res;
        }
        #endregion
        #endregion

        #region Binance future's events [COIN-M] [API streams][WebSocket]:
        #region Partial Depth:
        public void PartialDepth_COIN_M(string symbol = "BTCUSD")
        {
            var webSocket = new WebSocket4Net.WebSocket($"wss://dstream.binance.com/stream?streams={symbol.ToLower()}@depth10@100ms");

            webSocket.Opened += (sender, e) => logger.LogInfo("WebSocket connected! Start Partial @depth10 [FUTURE Coin-M] process...");

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                var depth = JsonConvert.DeserializeObject<PartialDepthFuture>(e.Message);
                var asks = depth.Data.Asks.OrderBy(p => p[0]).Select(o => o[0]).ToList();
                var bids = depth.Data.Bids.OrderByDescending(p => p[0]).Select(o => o[0]).ToList();
                var a = decimal.Parse(asks.First(), CultureInfo.InvariantCulture);
                var b = decimal.Parse(bids.First(), CultureInfo.InvariantCulture);
                OnTickFuture(new TickEventArgsFutures
                {
                    Ask = a,
                    Bid = b,
                    Symbol = symbol
                });
            };

            webSocket.Error += (sender, e) => logger.LogError($"WebSocket [PartialDepth_COIN_M()] [FUTURE Coin-M] error: {e.Exception.Message}");
            webSocket.Closed += (s, e) =>
            {
                Task.Delay(5000).ContinueWith(_ =>
                {
                    ReconectCount++;
                    if (ReconectCount > 5)
                    {
                        // Can not open WebSocket;
                        return;
                    }
                    logger.LogInfo(ViewId + " Reconect to WebSocket #");

                    // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                    if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                    timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                    logger.LogWarning($"WebSocket [PartialDepth_COIN_M()] [Closed] [FUTURE Coin-M] and reconnect again [during Task.Delay]..");
                    ConnectToDepthStream();
                });
                logger.LogWarning($"WebSocket [PartialDepth_COIN_M()] [Closed] [FUTURE Coin-M] and reconnect again..");
                ConnectToDepthStream();
            };
            webSocket.Open();
        }
        #endregion
        #endregion

        #region Closing Websocket Custom:
        private long startDepth = 0L, startBT = 0L, startAggTrd = 0L;
        private bool isCloseMDCustom = false, isCloseBTCustom = false, isCloseAggTrdCustom = false;
        WebSocket4Net.WebSocket depthWS = null, bookTikerWS = null, aggTrade = null;

        private void CloseWebsocket(string name)
        {
            switch (name)
            {
                case nameof(SubscribeBookTickerStream):
                    isCloseBTCustom = true;
                    bookTikerWS.Close();
                    break;
                case nameof(ConnectToDepthStream):
                    isCloseMDCustom = true;
                    depthWS.Close();
                    break;
                case nameof(SubscribeToAggTradeStream):
                    isCloseAggTrdCustom = true;
                    aggTrade.Close();
                    break;
            }
        }

        #endregion

        #region [CUSTOM MAMAGER SOCKET]:
        public class BinancePingPongManagerX
        {
            private WebSocket4Net.WebSocket _webSocket;
            private int _pingInterval = 3;
            private int _expectedPongDelay = 2;
            private readonly IConnectorLogger logger;

            public BinancePingPongManagerX(IConnectorLogger logger)
            {
                this.logger = logger;
            }

            public async void StartPingPong(WebSocket webSocket, int pingInterval = 180, int expectedPongDelay = 600)
            {
                _webSocket = webSocket;
                _pingInterval = pingInterval;
                _expectedPongDelay = expectedPongDelay;

                while (_webSocket.State == WebSocketState.Open)
                {
                    await Task.Delay(_pingInterval * 1000);

                    try
                    {
                        var pongReceivedTask = SendPingAndGetPong();

                        if (await Task.WhenAny(pongReceivedTask, Task.Delay(_expectedPongDelay * 1000)) != pongReceivedTask)
                        {
                            logger.LogInfo("(BinancePingPongManager.StartPingPong())=>Pong not received on time. Handle the disconnection or other logic here");
                            // Pong not received on time
                            // Handle the disconnection or other logic here
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, e.g., WebSocket closed unexpectedly
                        //Console.WriteLine($"Exception: {ex.Message}");
                    }
                }
            }

            private async Task SendPingAndGetPong()
            {
                var pongReceivedTaskSource = new TaskCompletionSource<bool>();

                _webSocket.Send("ping");

                _webSocket.MessageReceived += (sender, args) =>
                {
                    if (args.Message.Contains("pong"))
                        pongReceivedTaskSource.TrySetResult(true);
                };

                await pongReceivedTaskSource.Task;
            }
        }


        #endregion

        #region [!!!!!!!!!!!!!!!!!!!!!!!!]:
        public class BinancePingPongManager
        {
            private WebSocket4Net.WebSocket _webSocket;
            private int _pingInterval = 3;
            private int _expectedPongDelay = 2;
            private readonly IConnectorLogger _logger;

            public BinancePingPongManager(IConnectorLogger logger)
            {
                _logger = logger;
            }

            public async Task StartPingPong(WebSocket4Net.WebSocket webSocket, int pingInterval = 180, int expectedPongDelay = 600)
            {
                _webSocket = webSocket;
                _pingInterval = pingInterval;
                _expectedPongDelay = expectedPongDelay;

                while (_webSocket.State == WebSocketState.Open)
                {
                    await Task.Delay(_pingInterval * 1000);

                    try
                    {
                        var pingPayload = Guid.NewGuid().ToString(); // Генеруємо унікальний пінг-пейлод
                        var pongReceivedTask = SendPingAndGetPong(pingPayload);

                        if (await Task.WhenAny(pongReceivedTask, Task.Delay(_expectedPongDelay * 1000)) != pongReceivedTask)
                        {
                            _logger.LogInfo("(BinancePingPongManager.StartPingPong())=>Pong not received on time. Handle the disconnection or other logic here");
                            // Pong not received on time
                            // Handle the disconnection or other logic here
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions, e.g., WebSocket closed unexpectedly
                        _logger.LogError($"Exception: {ex.Message}");
                    }
                }
            }

            private async Task SendPingAndGetPong(string pingPayload)
            {
                var pongReceivedTaskSource = new TaskCompletionSource<bool>();

                _webSocket.Send($"{{\"ping\":{pingPayload}}}"); // Відправляємо пінг із унікальним пінг-пейлодом

                _webSocket.MessageReceived += (sender, args) =>
                {
                    if (args.Message.Contains("pong") && args.Message.Contains(pingPayload))
                        pongReceivedTaskSource.TrySetResult(true);
                };

                await pongReceivedTaskSource.Task;
            }
        }

        #endregion


    }
}


