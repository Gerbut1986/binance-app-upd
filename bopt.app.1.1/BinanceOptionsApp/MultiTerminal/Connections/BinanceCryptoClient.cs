using BinanceOptionApp;
using BinanceOptionsApp;
using Helpers.Extensions;
using Models.Algo;
using MultiTerminal.Connections.API.Spot;
using MultiTerminal.Connections.Details.Binance;
using MultiTerminal.Connections.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Threading;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace MultiTerminal.Connections
{
    public class BinanceCryptoClient : BaseCryptoClient
    {

        private DispatcherTimer timerEvent; // TIMER ***


        #region [onTick() TIMER FOR LEVEL_2 STREAM SYNCHRONIZATION]:
        private async void OnTick(object sender, EventArgs e)
        {
            ask_s = BestAskSpot; bid_s = BestBidSpot;
           // ask_s = LastAsk; bid_s = LastBid;

            await Task.Run(() =>
            {
                if (tas != null)
                {
                    long timeSynchronization = 0;

                    lock (lockObject)
                    {
                        if (ask_s != PrevAsk || bid_s != PrevBid || tas != PrevTAS)
                        {
                            decimal v = 0; //decimal ask = 0, bid = 0;
                            decimal p = 0;

                            if (tas == PrevTAS) //
                            {
                                tas.Volume = 0;
                                tas.Ticket = 0;
                                tas.Price = 0;
                                tas.BuyerID = 0;
                                tas.SellerID = 0;
                                
                                tas.EventTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                var dt1 = long.Parse(tas.EventTime.ToString()).GetFullTime();
                                tas.EventDate = $"{dt1.Day}/{dt1.Month} {dt1.ToString("HH:mm:ss.ffffff")}";
                                
                             
                                    tas.Ask = ask_s;
                                    tas.Bid = bid_s;
                                    var dt = long.Parse(tas.EventTime.ToString()).GetFullTime();
                                    tas.EventDate = $"{dt.Day}/{dt.Month} {dt.ToString("HH:mm:ss.ffffff")}";
                                    tas.Id = (int)(++counter);
                                    tas.AskBuyMarket = 0;
                                    tas.AskBuyLimit = 0;
                                    tas.BidSellMarket = 0;
                                    tas.BidSellLimit = 0;
                                    tas.BoomBuyLimit = 0;
                                    tas.BoomSellLimit = 0;
                                    tas.BoomLimit = 0;
                                    tas.BoomMarket = 0;

                                    timeAndSale.Add(tas);
                                

                            }
                            else
                            {
                                v = tas.Volume;
                                p = tas.Price;
                                bool isBuyerLimit = tas.IsBuyLimit;
                                bool isSellerLimit = tas.IsSellLimit;
                                long timeLastDeal = (long)tas.DealTime;
                                long LastTimeLenta = (long)tas.EventTime;


                                try
                                {
                                            tas.Ask = ask_s;
                                            tas.Bid = bid_s;
                                            var fullTime = long.Parse(tas.EventTime.ToString()).GetFullTime();
                                            tas.EventDate = $"{fullTime.Day}/{fullTime.Month} {fullTime.ToString("HH:mm:ss.ffffff")}";
                                            //tas.EventTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                            tas.Id = (int)(++counter);
                                            //EvTime = DateTime.Now;

                                            if (p == ask_s)
                                            {
                                                if (isBuyerLimit) tas.AskBuyLimit = v;
                                                else tas.AskBuyMarket = v;
                                            }
                                            else if (p == bid_s)
                                            {
                                                if (isSellerLimit) tas.BidSellLimit = v;
                                                else tas.BidSellMarket = v;
                                            }
                                            else
                                            {
                                                if (isBuyerLimit && isSellerLimit) tas.BoomLimit = v;
                                                else if (isBuyerLimit && !isSellerLimit) tas.BoomBuyLimit = v;
                                                else if (!isBuyerLimit && isSellerLimit) tas.BoomSellLimit = v;
                                                else if (!isBuyerLimit && !isSellerLimit) tas.BoomMarket = v;

                                            }
                                            timeAndSale.Add(tas);
                                        
                                    zz.BuildZigZag(tas.Ask, tas.Bid, tas.EventTime, (ulong)tas.Id);

                                }
                                catch (NullReferenceException)
                                {

                                }



                            }

                            PrevAsk = ask_s;
                            PrevBid = bid_s;
                            PrevTAS = tas;
                        }
                    }

                }

            });
        }
        #endregion

        #region Class members (fields):
        public List<BinanceMarginNewOrder> PlacedOrders { get; set; } = new List<BinanceMarginNewOrder>();

        private string accType;
        public List<BinanceUserAsset> MargBalances = default;
        public List<Balance> Balances = default;
        public BinanceMarginAccountDetails MarginAccount = null;
        public AccountInfo SpotAccount = null;
        DateTime lastBalanceRequestTime = DateTime.MinValue;
        decimal lastBalance = 0;
        int ReconectCount = 0;
        Timer timerResetReconectCount;
        private HttpClient _httpClient;
        readonly long recvWindow = 5000;
        static long dTime = 0; // разница между временем сервера и клиента
        readonly Dictionary<string, WebSocketData> SubscribeList = new Dictionary<string, WebSocketData>();
        static decimal Ask_View { get; set; } = 0.0m;
        static decimal Bid_View { get; set; } = 0.0m;
        #endregion

        #region Constructor:
        public BinanceCryptoClient(IConnectorLogger logger, ManualResetEvent cancelToken, BinanceConnectionModel model) : base(logger, cancelToken, model)
        {
            // Створення таймера
            timerEvent = new DispatcherTimer();
            timerEvent.Interval = TimeSpan.FromMilliseconds(10); // 10 мілісекунд

            timerEvent.Tick += OnTick; // Додавання події Tick

            timerEvent.Start(); // Запуск таймера


            Type obj = logger.GetType();
            switch (obj.Name)
            {
                case nameof(TwoLegArbFutureUSD_M):
                    (logger as TwoLegArbFutureUSD_M).smc = this;
                    break;
                case nameof(TradeLatencyArbitrage):
                    (logger as TradeLatencyArbitrage).bcm = this;
                    break;
                case nameof(TradeScalper):
                    if (AccountTradeType == AccountTradeType.MARGIN)
                        (logger as TradeScalper).bmc = this;
                    else
                        (logger as TradeScalper).bsc = this;
                    break;
                case nameof(TradeZigZag):
                    (logger as TradeZigZag).bsc = this;
                    zz = new ZigZagSpot(timeAndSale);
                    break;
            }
            accType = AccountTradeType == AccountTradeType.SPOT ? "SPOT" : "MARGIN";
        }
        #endregion

        #region Get Balance 
        internal override decimal GetBalance()
        {
            lastBalance = GetBalance(BinanceOptionsApp.Models.TradeModel.currencySpot).Result;
            lastBalanceRequestTime = DateTime.UtcNow;
            return lastBalance;
        }
        static int cntCallBal = 0;
        static Stopwatch sw = new Stopwatch();
        public async Task<decimal> GetBalance(string Asset)
        {
            decimal balance = 0;
            try
            {
                if (AccountTradeType == AccountTradeType.SPOT)
                {
                    AccountInfo AI = await CallAsync<AccountInfo>(EndPoints.AccountInformation, $"recvWindow={recvWindow}");
                    Balances = AI.Balances;
                    SpotAccount = AI;
                    balance = decimal.Parse(AI.Balances.Single(b => b.Asset == Asset.ToUpper()).Free, CultureInfo.InvariantCulture);
                    var fullSymb = AI.Balances.Where(s => s.Asset.Contains("USDT")).ToList();
                }
                else if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    //cntCallBal++;

                    //var start = DateTime.Now;
                    BinanceMarginAccountDetails details = await CallAsync<BinanceMarginAccountDetails>(EndPoints.QueryCrossMarginAccountDetails, $"recvWindow={recvWindow}");
                    // var stop = DateTime.Now;                    
                    //logger.LogInfo($"Elapsed.Milliseconds: {(stop-start).TotalMilliseconds} times.");
                    //logger.LogInfo($"GetBalance(http request): {cntCallBal} times.");
                    MarginAccount = details;
                    MargBalances = details.userAssets;
                    balance = decimal.Parse(details.totalCollateralValueInUSDT, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception e)
            {
                // logger.LogError(ViewId + $" Balance Error: {e.Message}");
            }

            return balance;
        }
        #endregion

        #region Order's actions:
        public async Task<decimal> GetOpenOrder()
        {
            decimal balance = 0;
            try
            {
                if (AccountTradeType == AccountTradeType.SPOT)
                {
                    AccountInfo AI = await CallAsync<AccountInfo>(EndPoints.AccountInformation, $"recvWindow={recvWindow}");
                }
                else if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    var oo = await CallAsync<MarginOpenOrders>(EndPoints.MarginAccountOpenOrders, $"recvWindow={recvWindow}");
                    int w = 0;
                }
            }
            catch (Exception e)
            {
            }

            return balance;
        }
        public override OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs)
        {
            var args = $"symbol={symbol}&recvWindow={recvWindow}";
            if (AccountTradeType == AccountTradeType.SPOT)
            {

            }
            if (AccountTradeType == AccountTradeType.MARGIN)
            {
                var DeleteAllResult = CallAsync<DeleteOrdersResult>(EndPoints.MarginAccountDeleteOpenOrders, args).Result;

                if (DeleteAllResult.status == "CANCELED")
                {
                }
            }

            return new OrderCloseResult()
            {
                //ClosePrice = openResult.OpenPrice,
                //ExecutionTime = openResult.ExecutionTime,
                //Error = openResult.Error
            };
        }
        public override bool OrderDelete(string symbol, string orderId, string origClientOrderId)
        {
            try
            {

                var args = $"symbol={symbol.ToUpper()}&orderId={orderId}&origClientOrderId={origClientOrderId}&recvWindow={recvWindow}";

                if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    BinanceNewOrder o = CallAsync<BinanceNewOrder>(EndPoints.MarginAccountOrderDelete, args).Result;

                    if (o.status == "CANCELED")
                    {
                        return true;
                    }
                    else return false;

                }
                if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    var DeleteResult = CallAsync<BinanceMarginNewOrder>(EndPoints.MarginAccountOrderDelete, args).Result;

                    if (DeleteResult.Status == "CANCELED")
                    {
                    }
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
                var delResult = OrderDelete(symbol, orderId, origClientOrderId);

                if (delResult)
                {
                    var start = DateTime.UtcNow;
                    OrderOpenResult openRes = Open(symbol, newPrice, lot, FillPolicy.GTC, side, 0, 0, 0, OrderType.Limit, 0);
                    var end = DateTime.UtcNow;
                    if (openRes.Error == null)
                    {
                        return new OrderModifyResult
                        {
                            OpenPrice = openRes.OpenPrice,
                            Lot = lot.ToString(),
                            Side = side.ToString(),
                            ExecutionTime = end - start
                        };
                    }
                    else return new OrderModifyResult { Error = "Modify has NOT worked!Something went wrong..." };
                }
                else return new OrderModifyResult { Error = "Delete Order has NOT worked! Something went wrong..." };
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return new OrderModifyResult { Error = e.Message };
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

                if (AccountTradeType == AccountTradeType.SPOT)
                {
                    BinanceNewOrder o = CallAsync<BinanceNewOrder>(EndPoints.NewOrder, args).Result;

                    if (o.status == "FILLED")
                    {
                        string Id = o.orderId.ToString();
                        decimal OpenPrice = decimal.Parse(o.price, CultureInfo.InvariantCulture);
                        decimal Volume = decimal.Parse(o.executedQty, CultureInfo.InvariantCulture);

                        if (OpenPrice == 0 && o.fills != null)
                        {
                            decimal sumAmount = 0;
                            decimal sumAmountPrice = 0;

                            foreach (BinanceFill f in o.fills)
                            {
                                decimal p = decimal.Parse(f.price, CultureInfo.InvariantCulture);
                                decimal q = decimal.Parse(f.qty, CultureInfo.InvariantCulture);
                                sumAmount += q;
                                sumAmountPrice += q * p;
                            }

                            OpenPrice = sumAmountPrice / sumAmount;
                        }

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
                        return new OrderOpenResult() { Error = $"Order status = {o.status}" };
                    }

                }
                if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    BinanceMarginNewOrder o = CallAsync<BinanceMarginNewOrder>(EndPoints.MarginAccountNewOrder, args).Result;
                    PlacedOrders.Add(o);
                    if (o.Status == "FILLED")
                    {
                        string Id = o.OrderId.ToString();
                        decimal OpenPrice = decimal.Parse(o.Price, CultureInfo.InvariantCulture);
                        decimal Volume = decimal.Parse(o.ExecutedQty, CultureInfo.InvariantCulture);

                        if (OpenPrice == 0 && o.Fills != null)
                        {
                            decimal sumAmount = 0;
                            decimal sumAmountPrice = 0;

                            foreach (BinanceFill f in o.Fills)
                            {
                                decimal p = decimal.Parse(f.price, CultureInfo.InvariantCulture);
                                decimal q = decimal.Parse(f.qty, CultureInfo.InvariantCulture);
                                sumAmount += q;
                                sumAmountPrice += q * p;
                            }

                            OpenPrice = sumAmountPrice / sumAmount;
                        }

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
                        string Id = o.OrderId.ToString();
                        decimal OpenPrice = decimal.Parse(o.Price, CultureInfo.InvariantCulture);
                        decimal Volume = decimal.Parse(o.ExecutedQty, CultureInfo.InvariantCulture);

                        if (OpenPrice == 0 && o.Fills != null)
                        {
                            decimal sumAmount = 0;
                            decimal sumAmountPrice = 0;

                            foreach (BinanceFill f in o.Fills)
                            {
                                decimal p = decimal.Parse(f.price, CultureInfo.InvariantCulture);
                                decimal q = decimal.Parse(f.qty, CultureInfo.InvariantCulture);
                                sumAmount += q;
                                sumAmountPrice += q * p;
                            }

                            OpenPrice = sumAmountPrice / sumAmount;
                        }

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

        #region Subscribe & Unsubscribe methods
        public override void Subscribe(string symbol, string id, string algo = "")
        {
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

            if (SubscribeList.ContainsKey(quote.Symbol))
                SubscribeList[quote.Symbol] = data;
            else SubscribeList.Add(quote.Symbol, data);

            switch (algo)
            {
                case "TwoLegArbitrage":
                case "TickOptimizerOG":
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
                case "Margin":
                    SubscribePartialDepthStream(quote.Symbol);
                    break;
                default:
                    //ConnectToBookTickerStream(symbol);
                    //ConnectToDepthStream(quote.Symbol);
                    //SubscribeToTradeStream(quote.Symbol);
                    //GetQuotesBTCUSDT(quote.Symbol);
                    SubscribePartialDepthStream(quote.Symbol);
                    //GetTradeInfo(quote.Symbol);
                    //ConnectToBookTickerStream(quote.Symbol);
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
        #endregion

        #region StartUserDataStream()
        private async void StartUserDataStream()
        {
            try
            {
                BinanceEndPoint ep = (AccountTradeType == AccountTradeType.SPOT)
                    ? EndPoints.SpotCreateListenKey : EndPoints.MarginCreateListenKey;
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
                    //var ob = GetOrderBook("BTCUSDT");
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
        #endregion

        #region Start & Stop methods
        public override void Start()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-MBX-APIKEY", Key);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _IsLoggedIn = true;

            StartUserDataStream();
        }

        public override void Stop(bool wait)
        {
            _IsLoggedIn = false;
            OnLoggedOut();
        }
        #endregion

        #region API (api.binance.com)
        public async Task<T> CallAsync<T>(BinanceEndPoint endpoint, string parameters = null, bool firstCall = true)
        {
            string finalEndpoint = endpoint.Value + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (endpoint.isSigned)
            {
                string p = parameters + (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + GenerateTimeStamp(DateTime.Now.ToUniversalTime());
                var signature = GenerateSignature(Secret, p);
                finalEndpoint = $"{endpoint.Value}?{p}&signature={signature}";
            }

            string urlPath = "https://api.binance.com" + finalEndpoint;
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
                BinanceServerTime sTime = await CallAsync<BinanceServerTime>(EndPoints.CheckServerTime);
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

        #region Binance SPOT's/Margin's events [API streams]:
        #region Fields:
        //static List<Depth> MarketDepth { get; set; } = new List<Depth>();
        // Original:
        public static ConcurrentDictionary<decimal, MarketDepthUpdateSpot> marketDepthList =
            new ConcurrentDictionary<decimal, MarketDepthUpdateSpot>();
        // Copied:
        private static ConcurrentDictionary<decimal, MarketDepthUpdateSpot> marketDepthUpdate_Cpy
            = new ConcurrentDictionary<decimal, MarketDepthUpdateSpot>();

        private object lockObject = new object();

        public static List<BookTickerSpot> BookTickerSpot = new List<BookTickerSpot>();
        //public static List<BookTickerSpot> copyOfBookTickerSpot2 = new List<BookTickerSpot>();
        public List<TimeAndSale_BidAsk> timeAndSale = new List<TimeAndSale_BidAsk>();
        public List<TimeAndSale_BidAsk> timeAndSaleRaw = new List<TimeAndSale_BidAsk>();

        public static ConcurrentDictionary<long, BookTickerSpot> BookTickerSpotDictionary = new ConcurrentDictionary<long, BookTickerSpot>();

        private ZigZagSpot zz;// = new ZigZagSpot();
        private ulong counter = 0;
        private double standardDeviationBuy = 0;
        private double standardDeviationSell = 0;
        private decimal meanBuy = 0, meanSell = 0;
       // private static string PrevTAS { get; set; }
        private TimeAndSale_BidAsk PrevTAS { get; set; }
        private static decimal PrevAsk { get; set; }
        private static decimal PrevBid { get; set; }
        public static decimal LastAsk { get; set; }
        public static decimal LastBid { get; set; }
        private QuoteAskBid qAB = new QuoteAskBid();

        private static TimeAndSale_BidAsk tas;

        private TimeAndSale_BidAsk time_sale;
        private BookTickerSpot Pre_BookTickerSpot { get; set; }
        private object lockObject2 = new object();

        public static decimal BestAskSpot { get; set; }
        public static decimal BestBidSpot { get; set; }
        #endregion

        #region Test using Ping / Pong:
        private async void TryPing(string symbol, string endPoint)
        {
                using (var socket = new System.Net.WebSockets.ClientWebSocket())
                {
                    var uri = new Uri($"wss://stream.binance.com/stream?streams={symbol.ToLower()}{endPoint}"); // Замініть на свій шлях

                    await socket.ConnectAsync(uri, CancellationToken.None);

                    var pingInterval = TimeSpan.FromMinutes(3); // Інтервал між ping

                    while (socket.State == System.Net.WebSockets.WebSocketState.Open)
                    {
                        await Task.Delay(pingInterval);

                        ArraySegment<byte> pingBuffer = new ArraySegment<byte>(new byte[] { 0x89, 0x00 });
                        // Відправка ping-фрейму
                        await socket.SendAsync(pingBuffer,
                            System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);

                        // Очікування pong-фрейму
                        var pong = await socket.ReceiveAsync(pingBuffer, CancellationToken.None);

                        // Опрацювання pong, наприклад, перевірка вмісту або інші дії
                        // ...
                    }
                }
        }
        #endregion

        #region Get Market Depth [Http request] [https://api.binance.com/api/v3/depth?symbol=btcusdt&limit=5000]:
        private static int cntCallMD = 0;
        public async void CallMarketDepth(string symbol = "BTCUSDT", int limit = 5000)
        {
            logger.LogInfo($"CallMarketDepth() => {++cntCallMD} times. ");
            var sufix = $"{symbol.ToLower()}&limit={limit}";
            string depthEndpoint = $"https://api.binance.com/api/v3/depth?symbol=" + sufix;// + limit;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(depthEndpoint); // tut zlitae!!!! //28.02
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        var depthUpdate = JsonConvert.DeserializeObject<MarketDepthUpdate>(responseContent);

                        foreach (var ask in depthUpdate.asks)
                        {
                            try
                            {
                                FillOrUpdAsksBids(ask[0], ask[1], 1);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        foreach (var bid in depthUpdate.bids)
                        {
                            try
                            {
                                FillOrUpdAsksBids(bid[0], bid[1], 2);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        //CallMarketDepth(symbol);  //1
                    }
                }
                catch (HttpRequestException ex)
                {
                }
            }
            await Task.Delay(0);
        }
        #endregion

        #region Get Market Depth [WebSoket] [Up to date Depth] [wss://stream.binance.com:9443/ws/btcusdt@depth]:
        /// <summary>
        /// MARKET DEPTH(Order Book) Up to Date current MarketDepth [via WebSocket]:
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public async void ConnectToDepthStream(string symbol = "btcusdt")
        {
            var webSocket = new WebSocket4Net.WebSocket($"wss://stream.binance.com/stream?streams={symbol.ToLower()}@depth@1000ms");

            webSocket.Opened += (sender, e) => logger.LogInfo($"WebSocket connected! Start [{accType}] @depth@1000ms process...");

            webSocket.Error += (sender, e) =>
            {
                logger.LogError($"Error (on Error) ConnectToDepthStream()[{accType}]:\n{e.Exception.Message}");
            };

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                var depth = JsonConvert.DeserializeObject<DepthSpot>(e.Message);

                //var remove = marketDepthList.FirstOrDefault(d => d.Value.LastUpdateId > depth.data.FinalUpdateId).Value;
                //if (remove != null)
                //{
                //}
                //else
                //{
                foreach (var ask in depth.data.Asks)
                {
                    FillOrUpdAsksBids(ask[0], ask[1], 1);
                }
                foreach (var bid in depth.data.Bids)
                {
                    FillOrUpdAsksBids(bid[0], bid[1], 2);
                }
                //CallMarketDepth();
                //}
                if (marketDepthList.Count != 0)
                {
                    marketDepthUpdate_Cpy = GetSafeCopyOfMarketDepthList(symbol);
                    //var asks = GetSortAsks(marketDepthUpdate_Cpy).ToList();
                    //var bids = GetSortBids(marketDepthUpdate_Cpy).ToList();
                    //if (asks.Count != 0 && bids.Count != 0)
                    //{
                    //    Ask_View = asks[0];
                    //    Bid_View = bids[0];
                    //}

                }
            };

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
                    logger.LogError($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again [with Task.Delay]..");
                    ConnectToDepthStream();
                });
                logger.LogError($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again..");
                ConnectToDepthStream();
            };

            webSocket.Open();
            Task.Delay(50);
        }

        /// <summary>
        /// Copy general array marketDepthUpdate:
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>completed copy</returns>
        public ConcurrentDictionary<decimal, MarketDepthUpdateSpot> GetSafeCopyOfMarketDepthList(string symbol)
        {
            ConcurrentDictionary<decimal, MarketDepthUpdateSpot> copyOfMarketDepthList
                = new ConcurrentDictionary<decimal, MarketDepthUpdateSpot>();

            lock (lockObject)
            {
                foreach (var kvp in marketDepthList)
                {
                    copyOfMarketDepthList.TryAdd(kvp.Key, kvp.Value.DeepCopy());
                }
            }
            return copyOfMarketDepthList;
        }

        IEnumerable<decimal> GetSortAsks(ConcurrentDictionary<decimal, MarketDepthUpdateSpot> copyOfMarketDepthList)
        {
            IEnumerable<decimal> asks = default;
            try
            {
                asks = copyOfMarketDepthList.Where(kv => (kv.Value.Type == 1))
                    .OrderBy(s => s.Key).Select(a => a.Key).ToList();
                return asks;
            }
            catch (Exception ex) { logger.LogError(ex.Message); return null; }
        }

        IEnumerable<decimal> GetSortBids(ConcurrentDictionary<decimal, MarketDepthUpdateSpot> copyOfMarketDepthList)
        {
            IEnumerable<decimal> bids = default;
            try
            {
                bids = copyOfMarketDepthList.Where(kv => (kv.Value.Type == 2))
                    .OrderByDescending(s => s.Key).Select(a => a.Key).ToList();
                return bids;
            }
            catch (Exception ex) { logger.LogError(ex.Message); return null; }
        }
        #endregion

        #region Get Partial Depth [Top bids and asks, Valid are 5, 10, or 20]:
        public static List<List<string>> BidsP { get; set; }
        public static List<List<string>> AsksP { get; set; }
        public void SubscribePartialDepthStream(string symbol = "BTCUSDT")
        {
            var webSocket = new WebSocket4Net.WebSocket($"wss://stream.binance.com/stream?streams={symbol.ToLower()}@depth20@1000ms");

            webSocket.Opened += (sender, e) => logger.LogInfo($"WebSocket connected! Start Partial @depth20 [{accType}] process...");

            webSocket.Send("pong");
            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                var depth = JsonConvert.DeserializeObject<PartialDepthSpot>(e.Message);
                var asks = depth.Data.Asks.Select(o => o[0]).ToList();
                var bids = depth.Data.Bids.Select(o => o[0]).ToList();
                ask_s = BestAskSpot = decimal.Parse(asks.First(), CultureInfo.InvariantCulture);
                bid_s = BestBidSpot = decimal.Parse(bids.First(), CultureInfo.InvariantCulture);
                OnTick(new TickEventArgs
                {
                    Asks = AsksP = depth.Data.Asks,
                    Bids = BidsP = depth.Data.Bids,
                    Ask = BestAskSpot,//Ask_View,
                    Bid = BestBidSpot,//Bid_View,
                    Symbol = symbol
                }); ;
            };

            webSocket.Error += (sender, e) => logger.LogError($"WebSocket [SubscribePartialDepthStream()] [{accType}] error: {e.Exception.Message}");
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
                       logger.LogWarning($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again [during Task.Delay]..");
                       ConnectToDepthStream();
                   });
                logger.LogWarning($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again..");
                ConnectToDepthStream();
            };
            webSocket.Open();
        }
        #endregion

        #region Get Quotes [WebSoket] [wss://stream.binance.com:9443/ws/btcusdt@ticker]:
        public void GetQuotesBTCUSDT(string symbol = "BTCUSDT")
        {
            var webSocket = new WebSocket($"wss://stream.binance.com:9443/ws/{symbol.ToLower()}@ticker");

            webSocket.Error += (sender, e) =>
            {
                logger.LogError($"Error (on Error) GetQuotesBTCUSDT()[Quote]:\n{e.Exception.Message}");
            };

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                try
                {
                    var jsonData = e.Message;  // Assuming e.Message is a string containing the JSON data
                    var tickerData = JsonConvert.DeserializeObject<QuoteSpot>(jsonData);

                    var dt = tickerData.E.GetFullTime();
                    qAB.Ask = LastAsk = decimal.Parse(tickerData.a, CultureInfo.InvariantCulture);
                    qAB.Bid = LastBid = decimal.Parse(tickerData.b, CultureInfo.InvariantCulture);
                    qAB.AskVol = decimal.Parse(tickerData.A, CultureInfo.InvariantCulture);
                    qAB.BidVol = decimal.Parse(tickerData.B, CultureInfo.InvariantCulture);
                    qAB.EventTime = $"{dt.Day}/{dt.Month} {dt.ToString("HH:mm:ss.ffffff")}";
                    // marketDepthUpdate_Cpy = GetSafeCopyOfMarketDepthList(symbol);
                    //OnTick(new TickEventArgs
                    //{
                    //    Ask = decimal.Parse(tickerData.a, CultureInfo.InvariantCulture),
                    //    Bid = decimal.Parse(tickerData.b, CultureInfo.InvariantCulture),
                    //    Symbol = symbol
                    //});
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception (on MessageReceived) GetQuotesBTCUSDT()[Quote]:\n{ex.Message}");
                }
            };
            webSocket.Open();
           // Task.Delay(10);
        }
        #endregion

        #region BookTicker []:
        public static decimal bid_s = 0.0m, ask_s = 0.0m, ask_vol = 0.0m, bid_vol = 0.0m;
        long lastSearchTime = 0; int lastIndexS=0; long LastTimeBook, LastTimeLenta;

        public void ConnectToBookTickerStream(string symbol = "btcusdt")
        {
            var webSocket = new WebSocket4Net.WebSocket($"wss://stream.binance.com:9443/ws/{symbol.ToLower()}@bookTicker");

            webSocket.Opened += (sender, e) => logger.LogInfo("WebSocket connected! Start @bookTicker ...");

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += (sender, e) =>
            {
                var bookTicker = JsonConvert.DeserializeObject<BookTickerSpot>(e.Message);
                if (bookTicker != Pre_BookTickerSpot)
                {
                    LastTimeBook = bookTicker.SynchronizationTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    bookTicker.CustomTimeLenta = LastTimeLenta;
                    AddToDictionary(bookTicker);
                    // BookTickerSpot.Add(bookTicker);                    

                    bid_s = bookTicker.BestBidPrice;
                    ask_s = bookTicker.BestAskPrice;
                    ask_vol = bookTicker.BestAskQuantity;
                    bid_vol = bookTicker.BestBidQuantity;

                    Pre_BookTickerSpot = bookTicker;
                }

            };
            webSocket.Error += (sender, e) => logger.LogError($"ConnectToBookTickerStream [{accType}] [Error]: {e.Exception}");
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
                        logger.LogError($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again [with Task.Delay]..");
                        ConnectToBookTickerStream();
                    });
                logger.LogError($"WebSocket [ConnectToDepthStream()] [Closed] [{accType}] and reconnect again..");
                ConnectToBookTickerStream();
            };

            webSocket.Open();
        }
        public BookTickerSpot FindModelByTime(long targetTime)
        {
            var CopyBook = BookTickerSpot;

            lock (lockObject)
            {
                BookTickerSpot mod;
                // var foundModel = CopyBook.FirstOrDefault(model => model.SynchronizationTime >= targetTime && model.SynchronizationTime >= lastSearchTime);
                var foundModelInd = CopyBook.FindIndex(lastIndexS, model => model.SynchronizationTime >= targetTime);
                if (foundModelInd > 0)
                {
                    mod = CopyBook[foundModelInd];
                    //  lastSearchTime = foundModel.SynchronizationTime;
                    lastIndexS = foundModelInd;
                    return mod;
                }
                else return null;
            }
        }

        public static async Task<BookTickerSpot> FindModelByTimeAsync(long targetTime)
        {
            if (BookTickerSpotDictionary.TryGetValue(targetTime, out var foundModel))
            {
                return foundModel;
            }
            else
            {
                return null;
            }
        }

        public  BookTickerSpot FindModelByTime2(long targetTime)
        {
            lock (lockObject2)
            {
                // Отримати відсортований список за зростанням EventTime
                var sortedModels = BookTickerSpotDictionary.OrderBy(pair => pair.Value.SynchronizationTime)
                    .FirstOrDefault(pair => pair.Value.SynchronizationTime >= targetTime);  //Value.Data.EventTime);

                // Знайти перший об'єкт з EventTime більшим або рівним TargetTime
              
                //var foundModel = sortedModels.FirstOrDefault(pair => pair.Value.SynchronizationTime >= targetTime);
           
                // Повернути знайдений об'єкт або null, якщо не знайдено
                return sortedModels.Value;
            }
        }


        public void AddToDictionary(BookTickerSpot model)
        {
            lock (lockObject2)
            {
                BookTickerSpotDictionary.TryAdd(model.SynchronizationTime, model);
            }
        }
        #endregion

        #region Time and Sales [WebSoket] [wss://stream.binance.com:9443/ws/btcusdt@trade]:
        public async void SubscribeToTradeStream(string symbol = "btcusdt")
        {
            var webSocket = new WebSocket($"wss://stream.binance.com:9443/ws/{symbol.ToLower()}@trade");

            webSocket.Opened += (sender, e) =>
            {
                logger.LogInfo("Lenta has started...Start SubscribeToTradeStream @trade [{accType}] process...");
            };

            webSocket.Error += (sender, e) =>
            {
                logger.LogError($"Error (on Error) SubscribeToTradeStreamAsync()[{accType}]:\n{e.Exception.Message}");
            };

            int lastIndexS = 0;

            webSocket.DataReceived += (sender, e) =>
            {
                var msg = e.Data;
            };

            webSocket.EnableAutoSendPing = true;
            webSocket.AutoSendPingInterval = 180;

            webSocket.MessageReceived += async (sender, e) =>
            {
                var time_sale_update = JsonConvert.DeserializeObject<TimeAndSale>(e.Message);
                time_sale = ObjConverter.GetConcreteType(time_sale_update);
                time_sale.EventTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                timeAndSaleRaw.Add(time_sale);
                tas = timeAndSaleRaw.LastOrDefault();
  
                
                

            }; // TimeAndSale

            webSocket.Closed += (s, e) =>
            {
                if (symbol != "btcusdt") return;
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
                        //  logger.LogInfo(ViewId + " Reconect to WebSocket #");

                        // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                        if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                        timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);
                        SubscribeToTradeStream();
                    });
                    SubscribeToTradeStream();
                }
            };
            webSocket.Open();
            //Task.Delay(10);
        }
        #endregion

        #region Get Market Depth [Http request] [https://api.binance.com/api/v3/depth?symbol=btcusdt&limit=5000]:
        public async void GetTradeInfo(string symbol = "BTCUSDT")
        {
            string depthEndpoint = "https://api.binance.com/api/v3/myTradeInfo";//? symbol =" + symbol.ToLower();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(depthEndpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();

                        var tradeInfo = JsonConvert.DeserializeObject<MyTradeSpot>(responseContent);

                    }
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError($"{depthEndpoint} ~ {ex.Message}");
                }
            }
            await Task.Delay(0);
        }
        #endregion

        #region Auxiliary methods:
        void FillOrUpdAsksBids(string price, string vol, int type, long lastUpdateId = 0)
        {
            var key = decimal.Parse(price, CultureInfo.InvariantCulture);
            if (marketDepthList.ContainsKey(key))
            {
                var volume = decimal.Parse(vol, CultureInfo.InvariantCulture);
                if (volume == 0.0m)
                {
                    marketDepthList.TryRemove(key, out MarketDepthUpdateSpot val);
                }
                else
                {
                    try
                    {
                        var found = marketDepthList[key];
                        if (marketDepthList.ContainsKey(key))
                        {
                            marketDepthList[key] = new MarketDepthUpdateSpot
                            {
                                Volume = decimal.Parse(vol, CultureInfo.InvariantCulture),
                                Type = type,
                                LastTimeUpd = DateTime.Now.ToString("mm:ss.fff"),
                                //LastUpdateId = lastUpdateId
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = type == 1 ? "Ask" : "Bid";
                        logger.LogError($"{msg} error : {ex.Message}");
                    }
                }
            }
            else if (!marketDepthList.ContainsKey(key))
            {
                try
                {
                    var volume = decimal.Parse(vol, CultureInfo.InvariantCulture);
                    if (volume == 0.0m)
                    {
                    }
                    else
                    {
                        marketDepthList.TryAdd(key, new MarketDepthUpdateSpot
                        {
                            Volume = decimal.Parse(vol, CultureInfo.InvariantCulture),
                            Type = type,
                            LastTimeUpd = DateTime.Now.ToString("mm:ss.fff"),
                            // LastUpdateId = lastUpdateId
                        });
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static void FunStD(decimal SellPrice, decimal BuyPrice, decimal Ask, decimal Bid)
        {
            int i = (int)(SellPrice * 100);
            int ii = (int)(BuyPrice * 100);
            int iAsk = (int)(Ask * 100);
            int iBid = (int)(Bid * 100);

            if (iAsk >= 0 && iAsk < marketDepthList.Count && i >= 0 && i < marketDepthList.Count && i > iAsk)
            {
                //var volumeRange = marketDepthList
                //    .Where(entry => int.Parse(entry.Key) >= iAsk && int.Parse(entry.Key) <= i && entry.Value.Type == 1 && entry.Value.Volume > 0)
                //    .Select(entry => entry.Value.Volume);

                //if (volumeRange.Any())
                //{
                //   // meanSell = volumeRange.Average();
                //    double squaredDifferencesSum = volumeRange.Sum(volume => Math.Pow((double)(volume - meanSell), 2));
                //    double variance = squaredDifferencesSum / volumeRange.Count();
                //    standardDeviationSell = Math.Sqrt(variance);
                //}
                //else
                //{
                //    // В діапазоні немає елементів
                //}
            }
            else
            {
                // Індекси виходять за межі масиву
            }

            if (iBid >= 0 && iBid < marketDepthList.Count && ii >= 0 && ii < marketDepthList.Count && iBid > ii)
            {
                //var volumeRange = marketDepthList
                //    .Where(entry => int.Parse(entry.Key) >= ii && int.Parse(entry.Key) <= iBid && entry.Value.Type == 2 && entry.Value.Volume > 0)
                //    .Select(entry => entry.Value.Volume);

                //if (volumeRange.Any())
                //{
                //    //meanBuy = volumeRange.Average();
                //    double squaredDifferencesSum = volumeRange.Sum(volume => Math.Pow((double)(volume - meanBuy), 2));
                //    double variance = squaredDifferencesSum / volumeRange.Count();
                //    standardDeviationBuy = Math.Sqrt(variance);
                //}
                //else
                //{
                //    // В діапазоні немає елементів
                //}
            }
            else
            {
                // Індекси виходять за межі масиву
            }
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
            if (LastAsk != 0 && LastBid != 0)
            {
                decimal SumA = 0, SumB = 0;
                var a = LastAsk;
                var b = LastBid;
                var start = LastAsk + 1.2m;
                for (decimal i = start; i > LastBid - 1.2m; i -= 0.01m)
                {
                    var key = i;
                    if (marketDepthList.ContainsKey(key))
                    {
                        var elem = marketDepthList[key];
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
                zz.SumAsk = SumA; zz.SumBid = SumB;
            }
        }
        #endregion

        #region FunDepthProba1 & FunDepthProba2 methods:
        public int FunDepthProba1(int Tip)
        {
            int res = 0;

            if (LastAsk != 0 && LastBid != 0)
            {
                //decimal SumA = 0, SumB = 0;
                int R_A = 0, R_B = 0;
                decimal SuppP = 0, SuppV = 0, ResisP = 0, ResisV = 0;

                var a = LastAsk;
                var b = LastBid;
                var start = LastAsk + 100.01m;
                for (decimal i = start; i > LastBid - 100.01m; i -= 0.01m)
                {
                    var key = i;
                    if (marketDepthList.ContainsKey(key))
                    {
                        var elem = marketDepthList[key];
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
                if (Tip == 1) { zz.ResistancePrice = ResisP; zz.ResistanceVol = ResisV; res = R_A; }
                if (Tip == 2) { zz.SupportPrice = SuppP; zz.SupportVol = SuppV; res = R_B; }

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
            //        decimal i = decimal.Parse(kvp.Key.ToString(), CultureInfo.InvariantCulture); // конвертація стрінгу в decimal
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

            //    if (Tip == 1) { ZigZagSpot.ResistancePrice = ResisP; ZigZagSpot.ResistanceVol = ResisV; res = R_A; }
            //    if (Tip == 2) { ZigZagSpot.SupportPrice = SuppP; ZigZagSpot.SupportVol = SuppV; res = R_B; }
            //}

            return res;
        }
        #endregion
        #endregion
    }
}
