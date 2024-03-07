namespace MultiTerminal.Connections.Models
{
    using System;
    using System.Net;
    using System.Linq;
    using System.Text;
    using WebSocket4Net;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Threading;
    using Newtonsoft.Json.Linq;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using MultiTerminal.Connections.Details.Binance;

    internal class BinanceTestnetSpotClient : BaseCryptoClient
    {
        DateTime lastBalanceRequestTime = DateTime.MinValue;
        decimal lastBalance = 0;
        int ReconectCount = 0;
        Timer timerResetReconectCount;
        private HttpClient _httpClient;
        readonly long recvWindow = 5000;
        static long dTime = 0; // разница между временем сервера и клиента
        readonly Dictionary<string, WebSocketData> SubscribeList = new Dictionary<string, WebSocketData>();

        public BinanceTestnetSpotClient(IConnectorLogger logger, ManualResetEvent cancelToken, TestnetSpotConnectionModel model) : base(logger, cancelToken, model)
        {
        }

        internal override decimal GetBalance()
        {
            if ((DateTime.UtcNow - lastBalanceRequestTime).TotalSeconds >= 5)
            {
                lastBalance = GetBalance("USDT").Result;
                lastBalanceRequestTime = DateTime.UtcNow;
            }
            return lastBalance;
        }

        internal async Task<decimal> GetBalance(string Asset)
        {
            decimal balance = 0;
            try
            {
                if (AccountTradeType == AccountTradeType.SPOT)
                {
                    IEnumerable<Balance> Balances = await CallAsync<IEnumerable<Balance>>(EndPoints.AccountInformation, $"recvWindow={recvWindow}");
                    balance = decimal.Parse(Balances.Single(ua => ua.Asset == Asset).Free, CultureInfo.InvariantCulture);
                }
                else if (AccountTradeType == AccountTradeType.MARGIN)
                {
                    BinanceMarginAccountDetails details = await CallAsync<BinanceMarginAccountDetails>(EndPoints.QueryCrossMarginAccountDetails, $"recvWindow={recvWindow}");
                    //balance = details.userAssets.Single(ua => ua.asset == Asset).free;
                }
            }
            catch (Exception e)
            {
                logger.LogError(ViewId + $" Balance Error: {e.Message}");
            }

            return balance;
        }

        #region Orders processes:
        public override OrderCloseResult Close(string symbol, string orderId, decimal price, decimal volume, OrderSide side, int slippage, OrderType type, int lifetimeMs)
        {
            OrderSide closeSide = side == OrderSide.Buy ? OrderSide.Sell : OrderSide.Buy;
            volume = 1;
            var openResult = Open(symbol, price, volume,FillPolicy.FILL, closeSide, 0, slippage, 0, type, lifetimeMs);

            return new OrderCloseResult()
            {
                ClosePrice = openResult.OpenPrice,
                ExecutionTime = openResult.ExecutionTime,
                Error = openResult.Error
            };
        }
        public override bool OrderDelete(string symbol, string orderId, string origClientOrderId)
        {
            return false;
        }
        public override OrderModifyResult Modify(string symbol, string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot)
        {
            return new OrderModifyResult() { Error = "Not implemented" };
        }
        public override OrderOpenResult Open(string symbol, decimal price, decimal lot, FillPolicy policy, OrderSide side, int magic, int slippage, int track, OrderType type, int lifetimeMs)
        {
            try
            {
                DateTime begin = DateTime.UtcNow;
                OrderInformation order = null;

                string sPrice = lot.ToString(CultureInfo.InvariantCulture);
                string sAmount = lot.ToString(CultureInfo.InvariantCulture);
                string sSide = (side == OrderSide.Buy) ? "BUY" : "SELL";

                var args = "";
                if (type == OrderType.Limit)
                {
                    args = $"symbol={symbol}&side={sSide}&type=LIMIT&timeInForce={policy.ToString()}&quantity={sAmount}&price={sPrice}&recvWindow={recvWindow}";
                }
                else
                    args = $"symbol={symbol}&side={sSide}&type=MARKET&quantity={sAmount}&recvWindow={recvWindow}";

                if (AccountTradeType == AccountTradeType.SPOT)
                {
                    BinanceTestnetNewOrder o = CallAsync<BinanceTestnetNewOrder>(EndPoints.NewOrderT, args).Result;

                    if (o.status == "NEW")
                    {
                        string Id = o.orderId.ToString();
                        decimal OpenPrice = decimal.Parse(o.price, CultureInfo.InvariantCulture);
                        decimal Volume = decimal.Parse(o.executedQty, CultureInfo.InvariantCulture);

                        if (OpenPrice == 0)
                        {
                            decimal sumAmount = 0;
                            decimal sumAmountPrice = 0;

                            decimal p = price;
                            decimal q = lot;
                            sumAmount += q;
                            sumAmountPrice += q * p;

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
            {
                SubscribeList[quote.Symbol] = data;

            }
            else
            {
                SubscribeList.Add(quote.Symbol, data);
            }

            Subscribe(data);
        }
        public void Subscribe(WebSocketData data)
        {
            WebSocket wsDepth = new WebSocket($"wss://testnet.binance.vision/ws/{data.quote.Symbol.ToLower()}@depth");
            wsDepth.MessageReceived += (s, e) =>
            {
                try
                {
                    JObject depth = (JObject)JsonConvert.DeserializeObject(e.Message);
                    try
                    {
                        decimal b = decimal.Parse((string)((JArray)depth["a"])[0][0], CultureInfo.InvariantCulture);
                        data.quote.Bid = b;
                    }
                    catch { }
                    try
                    {
                        decimal a = decimal.Parse((string)((JArray)depth["b"])[0][0], CultureInfo.InvariantCulture);
                        data.quote.Ask = a;
                    }
                    catch { }

                    OnTick(data.quote);
                }
                catch { }
            };
            wsDepth.Closed += (s, e) =>
            {
                // logger.LogInfo($"{ViewId}: {data.quote.Symbol} - Websocket closed");

                if (!SubscribeList.ContainsKey(data.quote.Symbol)) return;

                if (SubscribeList[data.quote.Symbol].Active)
                {
                    Task.Delay(5000).ContinueWith(_ =>
                    {

                        ReconectCount++;
                        if (ReconectCount > 5)
                        {
                            //    logger.LogWarning(ViewId + " Can not open WebSocket");
                            return;
                        }
                        //  logger.LogInfo(ViewId + " Reconect to WebSocket #");

                        // если в течении минуты нет реконектов, то сбрасываем счётчик реконектов
                        if (timerResetReconectCount != null) { timerResetReconectCount.Dispose(); }
                        timerResetReconectCount = new Timer(__ => { ReconectCount = 0; timerResetReconectCount.Dispose(); }, null, 60 * 1000, 5000);

                        Subscribe(data);
                    });
                }
            };
            wsDepth.Error += (s, e) =>
            {
                logger.LogError($"{ViewId}: {data.quote.Symbol} - {e.Exception.Message}");
            };
            data.webSocket = wsDepth;
            wsDepth.Open();
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
        }
        private async void StartUserDataStream()
        {
            try
            {
                BinanceEndPoint ep = (AccountTradeType == AccountTradeType.SPOT) ? EndPoints.SpotCreateListenKeyTest : EndPoints.MarginCreateListenKey;
                BinanceListenKey o = await CallAsync<BinanceListenKey>(ep);
                if (!string.IsNullOrEmpty(o.listenKey))
                {
                    OnLoggedIn();
                }
                else
                {
                    logger.LogError(ViewId + $" listenKey is null");
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string finalEndpoint = endpoint.Value + (string.IsNullOrWhiteSpace(parameters) ? "" : $"?{parameters}");

            if (endpoint.isSigned)
            {
                string p = parameters + (!string.IsNullOrWhiteSpace(parameters) ? "&timestamp=" : "timestamp=") + GenerateTimeStamp(DateTime.Now.ToUniversalTime());
                var signature = GenerateSignature(Secret, p);
                finalEndpoint = $"{endpoint.Value}?{p}&signature={signature}";
            }

            string urlPath = "https://testnet.binance.vision" + finalEndpoint;
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
        #endregion

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

        #region signature
        public static class SignatureHelper
        {
            /// <summary>Signs the given source with the given key using HMAC SHA256.</summary>
            public static string Sign(string source, string key)
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                using (HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes))
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

                    byte[] hash = hmacsha256.ComputeHash(sourceBytes);

                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        private static string Signature(string apiSecret)
        {
            // Basic 
            Dictionary<string, object> basicParameters = new Dictionary<string, object> {
                { "timestamp", 1578963600000 }
            };

            string basicQueryString = BuildQueryString(basicParameters);
            string basicSignature = SignatureHelper.Sign(basicQueryString, apiSecret);

            // Complex
            Dictionary<string, object> complexParameters = new Dictionary<string, object> {
                { "symbol", "LTCBTC" },
                { "side", "BUY" },
                { "type", "LIMIT" },
                { "timeInForce", "GTC" },
                { "quantity", 1 },
                { "price", 0.1 },
                { "recvWindow", 5000 },
                { "timestamp", 1499827319559 },
            };

            string complexQueryString = BuildQueryString(complexParameters);
            string complexSignature = SignatureHelper.Sign(complexQueryString, apiSecret);
            return complexSignature;
        }

        /// <summary>Builds a URL encoded query string for the given parameters.</summary>
        private static string BuildQueryString(Dictionary<string, object> queryParameters)
        {
            return string.Join("&", queryParameters.Select(kvp =>
                string.Format("{0}={1}", kvp.Key, System.Web.HttpUtility.UrlEncode(kvp.Value.ToString()))));
        }
        #endregion
    }
}
